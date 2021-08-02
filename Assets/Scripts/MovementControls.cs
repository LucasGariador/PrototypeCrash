using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControls : MovementController
{
    [Header("Dash key")]
    public KeyCode DashCode = KeyCode.Space;

    float _horizontal;
    float _vertical;
    public override void Initialize()
    {
        _horizontal = 0f;
        _vertical = 0f;
    }
    //Gets the imput from WASD  or the arrow keys, is the old Unity Input System
    private void MoveInput()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        MoveDirection = new Vector3(_horizontal, 0f, _vertical).normalized;
    }

    private void DashInput()
    {
        if (Input.GetKeyDown(DashCode))
        {
            OnDash();
        }
    }

    private void Update()
    {
        MoveInput();
        DashInput();
    }
}
