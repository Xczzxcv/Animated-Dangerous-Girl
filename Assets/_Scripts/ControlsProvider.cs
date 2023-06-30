using System;
using UnityEngine;

internal class ControlsProvider : MonoBehaviour, 
    ICharacterControlsProvider, ITargetControlsProvider
{
    public event Action Jump;
    public event Action Shoot;
    public event Action<Vector3> TargetShift;

    private ControlsView _controlsView;

    public void Init(ControlsView controlsView)
    {
        _controlsView = controlsView;
        _controlsView.JumpBtnClick += OnJump;
        _controlsView.ShootBtnClick += OnShoot;
    }

    private void OnJump() => Jump?.Invoke();

    private void OnShoot() => Shoot?.Invoke();

    private void Update()
    {
        UpdateJoystick();
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnShoot();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump();
        }
    }

    private void UpdateJoystick()
    {
        var joystickDir = _controlsView.JoystickDir;
        if (joystickDir == Vector2.zero)
        {
            return;
        }

        TargetShift?.Invoke(new Vector3(
            joystickDir.x,
            0,
            joystickDir.y
        ));
    }
}