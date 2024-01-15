using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterInput : MonoBehaviour
{
    #region Required Components
    [HideInInspector]
    public CharacterMovement Movement;
    #endregion

    private void Awake()
    {
        #region Required Components
        Movement = GetComponent<CharacterMovement>();
        #endregion

    }

    private Action _continuousInputs;
    private int _moveDirection;


    private void Update()
    {
        _continuousInputs?.Invoke(); // Continous inputs like Holding the jump button

        // Movement
        if (_moveDirection == 0) Movement.Decelerate();
        else Movement.Move(_moveDirection);
    }

    public void InputAxis(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();

        _moveDirection = Mathf.Approximately(input.x, 0) ? 0 : input.x > 0 ? 1 : -1; // round the input.x
    }
    public void InputA(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.Log("A Down");
            Movement.Jump();

            _continuousInputs += Movement.HoldJump;
        }
        else if(ctx.canceled)
        {
            Debug.Log("A Up");
            Movement.RealeaseJump();

            _continuousInputs -= Movement.HoldJump;
        }
    }
    public void InputB(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.Log("B Down");
        }
        else if (ctx.canceled)
        {
            Debug.Log("B Up");
        }
    }
    public void InputX(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.Log("X Down");
        }
        else if (ctx.canceled)
        {
            Debug.Log("X Up");
        }
    }
    public void InputY(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.Log("Y Down");
        }
        else if (ctx.canceled)
        {
            Debug.Log("Y Up");
        }
    }

}
