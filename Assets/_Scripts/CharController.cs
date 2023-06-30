using System;
using UnityEngine;

internal class CharController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private RifleController rifle;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float maxNoRotationAngle;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float jumpDuration;
    [SerializeField]
    private AnimationCurve jumpAnimationCurve;

    private static readonly int IsRunning = Animator.StringToHash("Is Running");
    private static readonly int IsAiming = Animator.StringToHash("Is Aiming");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Rotation = Animator.StringToHash("Rotation");
    private static readonly int Shoot = Animator.StringToHash("Shoot");

    private TargetController _target;
    private ICharacterControlsProvider _characterControlsProvider;
    private float _startJumpTime = float.NegativeInfinity;
    public Transform Transform { get; private set; }

    private bool IsAlreadyJumping => Time.time < _startJumpTime + jumpDuration;

    private void Awake()
    {
        Transform = transform;
    }

    public void Init(TargetController target, ICharacterControlsProvider characterControlsProvider)
    {
        _target = target;
        _characterControlsProvider = characterControlsProvider;
        _characterControlsProvider.Jump += OnJump;
        _characterControlsProvider.Shoot += OnShoot;
    }

    private void OnJump()
    {
        if (!IsAlreadyJumping)
        {
            _startJumpTime = Time.time;
        }
        
        animator.SetTrigger(Jump);
    }

    private void OnShoot()
    {
        rifle.Shoot();
        animator.SetTrigger(Shoot);
    }

    private void Update()
    {
        var targetPosition = _target.Transform.position;
        targetPosition.y = 0;
        
        var currPos = Transform.position;
        currPos.y = 0;

        var posDiff = targetPosition - currPos;
        var atTarget = posDiff == Vector3.zero;
        var isRotating = UpdateRotation(atTarget, posDiff);
        UpdatePosition(isRotating, atTarget, currPos, targetPosition);
        UpdateJumping();

        animator.SetBool(IsRunning, !atTarget && !isRotating);
        animator.SetBool(IsAiming, atTarget && !isRotating);
    }

    private void UpdateJumping()
    {
        if (!IsAlreadyJumping)
        {
            return;
        }

        var t = Mathf.InverseLerp(
            _startJumpTime,
            _startJumpTime + jumpDuration,
            Time.time
        );
        var animCurveValue = jumpAnimationCurve.Evaluate(t);
        var y = animCurveValue * jumpHeight;
        var currPos = Transform.position;
        Transform.position = new Vector3(currPos.x, y, currPos.z);
    }

    private void UpdatePosition(bool isRotating, bool atTarget, Vector3 currPos, Vector3 targetPosition)
    {
        if (isRotating || atTarget || IsAlreadyJumping)
        {
            return;
        }

        var posDelta = Time.deltaTime * speed;
        Transform.position = Vector3.MoveTowards(currPos, targetPosition, posDelta);
    }

    private bool UpdateRotation(bool atTarget, Vector3 posDiff)
    {
        int animRotationValue = 0;
        if (atTarget)
        {
            animator.SetFloat(Rotation, animRotationValue);
            return false;
        }

        var newRotation = Quaternion.LookRotation(posDiff, Vector3.up).normalized;
        var diffAngle = Quaternion.Angle(Transform.rotation, newRotation);
        if (Math.Abs(diffAngle) < maxNoRotationAngle)
        {
            animator.SetFloat(Rotation, animRotationValue);
            return false;
        }

        var maxRotationAngle = rotationSpeed * Time.deltaTime;
        Transform.rotation = Quaternion.RotateTowards(Transform.rotation, newRotation, maxRotationAngle);

        animRotationValue = Math.Sign(diffAngle);
        animator.SetFloat(Rotation, animRotationValue);
        return true;
    }
}