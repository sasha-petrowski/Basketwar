using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    #region Required Components
    [HideInInspector]
    public Character Controller;
    private Rigidbody2D _rb;
    #endregion

    [Header("Horizontal Movement")]
    [Min(1)]
    public float MaxSpeed;
    [Min(1)]
    public float Acceleration;
    [Min(1)]
    public float Decceleration;

    [Header("Jumping")]


    [Header("Refs")]
    [SerializeField]
    private ColliderEvent _groundCollider;

    [Header("Feedback Events")]
    public UnityEvent OnTouchGround;



    // Logic variables
    private bool _hasTouchedGround;

    private void Awake()
    {
        #region Required Components
        Controller = GetComponent<Character>();
        _rb = GetComponent<Rigidbody2D>();
        #endregion

        #region Subscribe to listeners
        _groundCollider.OnTriggerEnter += TouchGround;
        #endregion
    }

    public void InputAxis(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        if(input.x == 0) // if no input Decellerate
        {
            if (_rb.velocity.x == 0) return; // don't deccelerate when not moving

            float speed = _rb.velocity.x - Decceleration * MaxSpeed * Mathf.Sign(_rb.velocity.x) * Time.deltaTime;

            _rb.velocity = new Vector2(speed, _rb.velocity.y);
        }
        else // Accelerate
        {
            if (_rb.velocity.x * input.x > MaxSpeed) return; // don't accelerate over max speed

            float speed = _rb.velocity.x + Acceleration * MaxSpeed * input.x * Time.deltaTime;

            _rb.velocity = new Vector2(speed, _rb.velocity.y);
        }
    }
    private void TouchGround(Collider2D ground)
    {
        if(_hasTouchedGround == false)
        {
            OnTouchGround?.Invoke();
        }

        _hasTouchedGround = true;
    }





}
