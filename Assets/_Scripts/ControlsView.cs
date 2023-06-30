using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class ControlsView : UIBehaviour
{
    [SerializeField]
    private Button jumpBtn;
    [SerializeField]
    private Button shootBtn;
    [SerializeField]
    private Joystick joystick;

    public event Action JumpBtnClick;
    public event Action ShootBtnClick;
    public Vector2 JoystickDir => joystick.Direction;

    protected override void Awake()
    {
        jumpBtn.onClick.AddListener(OnJumpBtnClick);
        shootBtn.onClick.AddListener(OnShootBtnClick);
    }

    private void OnJumpBtnClick()
    {
        JumpBtnClick?.Invoke();
    }

    private void OnShootBtnClick()
    {
        ShootBtnClick?.Invoke();
    }
}