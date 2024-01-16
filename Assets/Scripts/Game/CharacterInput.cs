using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterInput : MonoBehaviour
{
    public Action ADown, BDown, XDown, YDown;
    public Action AHold, BHold, XHold, YHold;
    public Action AUp, BUp, XUp, YUp;
    public Action<int> MoveDirection;

    private Action _continuousInputs;


    private void Update()
    {
        _continuousInputs?.Invoke(); // Continous inputs like Holding the jump button
    }

    public void InputAxis(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();

        MoveDirection?.Invoke(Mathf.Approximately(input.x, 0) ? 0 : input.x > 0 ? 1 : -1); // round the input.x
    }
    public void InputA(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.Log("A Down");
            ADown?.Invoke();

            if (AHold != null) _continuousInputs += AHold.Invoke;
        }
        else if(ctx.canceled)
        {
            Debug.Log("A Up");
            AUp?.Invoke();

            if (AHold != null) _continuousInputs -= AHold.Invoke;
        }
    }
    public void InputB(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.Log("B Down");
            BDown?.Invoke();

            if(BHold != null) _continuousInputs += BHold.Invoke;
        }
        else if (ctx.canceled)
        {
            Debug.Log("B Up");
            BUp?.Invoke();

            if (BHold != null) _continuousInputs -= BHold.Invoke;
        }
    }
    public void InputX(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.Log("X Down");
            XDown?.Invoke();

            if (XHold != null) _continuousInputs += XHold.Invoke;
        }
        else if (ctx.canceled)
        {
            Debug.Log("X Up");
            XUp?.Invoke();

            if (XHold != null) _continuousInputs -= XHold.Invoke;
        }
    }
    public void InputY(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.Log("Y Down");
            YDown?.Invoke();

            if (YHold != null) _continuousInputs += YHold.Invoke;
        }
        else if (ctx.canceled)
        {
            Debug.Log("B Up");
            YUp?.Invoke();

            if (YHold != null) _continuousInputs -= YHold.Invoke;
        }
    }

}
