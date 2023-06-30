using UnityEngine;

internal class TargetController : MonoBehaviour
{
    [SerializeField]
    private float maxShiftDistance;
    
    public Transform Transform { get; private set; }

    private ITargetControlsProvider _targetControlsProvider;
    private CharController _character;

    private void Awake()
    {
        Transform = transform;
    }

    public void Init(ITargetControlsProvider targetControlsProvider, CharController character)
    {
        _targetControlsProvider = targetControlsProvider;
        _character = character;
        _targetControlsProvider.TargetShift += OnNewTargetShift;
    }

    private void OnNewTargetShift(Vector3 newShift)
    {
        var realWorldShift = newShift * maxShiftDistance;
        var newTargetPos = _character.Transform.position + realWorldShift;
        Transform.position = newTargetPos;
    }
}