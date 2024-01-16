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
public class CharacterMovement : MonoBehaviour
{
    #region Required Components
    [HideInInspector]
    public Character Character;
    [HideInInspector]
    public CharacterFeedback Feedback;
    private Rigidbody2D _rb;
    #endregion

    [Header("Horizontal Movement")]
    [Min(1)]
    public float MaxSpeed;
    [Min(1)]
    public float Acceleration;
    [Min(1)]
    public float Decceleration;

    [Header("Refs")]
    [SerializeField]
    private ColliderUtility _groundCollider;


    // Logic variables
    private int _direction;

    private void Awake()
    {
        #region Required Components
        Character = GetComponent<Character>();
        _rb = GetComponent<Rigidbody2D>();
        #endregion

        #region Subscribe to listeners
        //Inputs
        CharacterInput input = GetComponent<CharacterInput>();

        input.MoveDirection += OnDirectionChange;
        #endregion
    }

    private void FixedUpdate()
    {
        if (_direction == 0 | !Character.CanMove) // Decelerate if no move Input or can't move
        {
            Decelerate();
        }
        else
        {
            Move();
        }
    }
    private void OnDirectionChange(int direction) 
    {
        _direction = direction;
    }
    public void Decelerate()
    {
        if(_groundCollider.TouchCount > 0)
        {
            if (_rb.velocity.x == 0) return;

            float speed = _rb.velocity.x - Decceleration * MaxSpeed * Mathf.Sign(_rb.velocity.x) * Time.deltaTime;

            _rb.velocity = new Vector2(speed, _rb.velocity.y);
        }
    }
    public void Move()
    {
        if (_rb.velocity.x * _direction > MaxSpeed) return; // don't accelerate over max speed

        float speed = _rb.velocity.x + Acceleration * MaxSpeed * _direction * Time.deltaTime;

        _rb.velocity = new Vector2(speed, _rb.velocity.y);
    }
}
