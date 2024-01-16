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
[RequireComponent(typeof(CharacterFeedback))]
public class CharacterJump: MonoBehaviour
{
    #region Required Components
    [HideInInspector]
    public Character Character;
    [HideInInspector]
    public CharacterFeedback Feedback;
    private Rigidbody2D _rb;
    #endregion

    [Header("Jumping")]
    public float JumpForce;
    public float JumpGravity;

    [Header("Refs")]
    [SerializeField]
    private ColliderUtility _groundCollider;

    // Logic variables
    private bool _hasTouchedGround;

    private void Awake()
    {
        #region Required Components
        Character = GetComponent<Character>();
        Feedback = GetComponent<CharacterFeedback>();
        _rb = GetComponent<Rigidbody2D>();
        #endregion

        #region Subscribe to listeners
        //Inputs
        CharacterInput input = GetComponent<CharacterInput>();

        input.ADown += Jump;
        input.AHold += HoldJump;
        input.AUp += RealeaseJump;


        // Game
        _groundCollider.OnTriggerEnter += TouchGround;
        #endregion
    }

    private void TouchGround(Collider2D ground)
    {
        if(_hasTouchedGround == false)
        {
            Feedback.OnTouchGround?.Invoke();
        }

        _hasTouchedGround = true;
    }

    public void Jump()
    {
        if (!Character.CanMove) return;

        Character.OnDroped();

        if (_hasTouchedGround | _groundCollider.TouchCount > 0)
        {
            _hasTouchedGround = false;

            _rb.velocity += new Vector2(0, JumpForce);

            _rb.gravityScale = JumpGravity;
            _rb.drag = 0;
        }
    }
    public void HoldJump()
    {
        if (!Character.CanMove) return;

        if (_rb.velocity.y > 0) // only apply reduced gravity if going up
        {
            _rb.gravityScale = JumpGravity;
            _rb.drag = 0;
        }
        else
        {
            _rb.gravityScale = 1;
            _rb.drag = 1;
        }
    }
    public void RealeaseJump()
    {
        if (!Character.CanMove) return;

        _rb.gravityScale = 1;
        _rb.drag = 1;
    }
}
