using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterInput))]
public class CharacterMovement : MonoBehaviour
{
    #region Required Components
    [HideInInspector]
    public Character Character;
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
    public float JumpForce;
    [Range(0f, 1f)]
    public float JumpGravity;

    [Header("Refs")]
    [SerializeField]
    private ColliderUtility _groundCollider;

    [Header("Feedback Events")]
    public UnityEvent OnTouchGround;



    // Logic variables
    private bool _hasTouchedGround;

    private void Awake()
    {
        #region Required Components
        Character = GetComponent<Character>();
        _rb = GetComponent<Rigidbody2D>();
        #endregion

        #region Subscribe to listeners
        //Inputs
        CharacterInput input = GetComponent<CharacterInput>();

        input.MoveAction += Move;

        input.ADown += Jump;
        input.AHold += HoldJump;
        input.AUp += RealeaseJump;


        // Game
        _groundCollider.OnTriggerEnter += TouchGround;
        #endregion
    }

    public void Decelerate()
    {
        if (_rb.velocity.x == 0) return; // don't deccelerate when not moving

        float speed = _rb.velocity.x - Decceleration * MaxSpeed * Mathf.Sign(_rb.velocity.x) * Time.deltaTime;

        _rb.velocity = new Vector2(speed, _rb.velocity.y);
    }
    public void Move(int direction)
    {
        if (direction == 0) // Decelerate if no move Input
        {
            Decelerate();
            return;
        }

        if (_rb.velocity.x * direction > MaxSpeed) return; // don't accelerate over max speed

        float speed = _rb.velocity.x + Acceleration * MaxSpeed * direction * Time.deltaTime;

        _rb.velocity = new Vector2(speed, _rb.velocity.y);
    }

    private void TouchGround(Collider2D ground)
    {
        if(_hasTouchedGround == false)
        {
            OnTouchGround?.Invoke();
        }

        _hasTouchedGround = true;
    }

    public void Jump()
    {
        if(_hasTouchedGround | _groundCollider.TouchCount > 0)
        {
            _hasTouchedGround = false;

            _rb.velocity += new Vector2(0, JumpForce);

            _rb.gravityScale = JumpGravity;
        }
    }
    public void HoldJump()
    {
        if(_rb.velocity.y > 0) // only apply reduced gravity if going up
        {
            _rb.gravityScale = JumpGravity;
        }
        else
        {
            _rb.gravityScale = 1;
        }
    }
    public void RealeaseJump()
    {
        _rb.gravityScale = 1;
    }
}
