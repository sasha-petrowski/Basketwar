using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;
using UnityEngine.Windows;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterInput))]
[RequireComponent(typeof(CharacterFeedback))]
public class CharacterDash: MonoBehaviour
{
    #region Required Components
    [HideInInspector]
    public Character Character;
    [HideInInspector]
    public CharacterFeedback Feedback;
    private Rigidbody2D _rb;
    #endregion

    [Header("Dashing")]
    public float Velocity;
    public float Duration;
    public float Cooldown;

    [Header("Dash Stun")]
    public int StunLevel;

    [Header("Refs")]
    [SerializeField]
    private ColliderUtility _playerCollider;

    public bool Dashing {  get; private set; }

    // Logic Vars
    private bool _cooldown = true;
    private int _nonZeroDirection = 1;


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

        input.BDown += Dash;

        input.MoveDirection += OnDirectionChange;

        _playerCollider.OnTriggerEnter += DashCollision;
        #endregion
    }
    private void OnDirectionChange(int direction)
    {
        if(direction != 0) _nonZeroDirection = direction;
    }

    public void DashCollision(Collider2D collision)
    {
        if(Dashing && collision.TryGetComponent(out CharacterReference reference) && reference.Character.TryGetComponent(out CharacterStun stun))
        {
            stun.Stun(StunLevel);
            Debug.Log("Dash Collision");
        }
    }

    public void Dash()
    {
        if (!Character.CanMove) return;

        if(_cooldown)
        {
            Dashing = true;

            // disable collisions with players
            _playerCollider.Collider.isTrigger = true;

            // Off cooldown
            _cooldown = false;

            // Timer to End dash & Reset cooldown
            StartCoroutine(Utility.WaitFor(Duration, EndDash));
            StartCoroutine(Utility.WaitFor(Cooldown, ResetCooldown));

            // Set velocity
            _rb.velocity = new Vector2(Velocity * _nonZeroDirection, 0);

            // Player can't move & Disable gravity
            Character.CanMoveCount++;
            _rb.gravityScale = 0;
        }
    }
    private void EndDash()
    {
        Dashing = false;

        // reactive collisions with players
        _playerCollider.Collider.isTrigger = false;

        // Player can move & Restore gravity
        Character.CanMoveCount--;
        if (!Character.CanMove) return;
        _rb.gravityScale = 1;

        #region Set velocity as MaxSpeed
        float velocity = _nonZeroDirection;
        if(TryGetComponent(out CharacterMovement movement))
        {
            velocity *= movement.MaxSpeed;
        }
        _rb.velocity = new Vector2(velocity, 0);
        #endregion

    }
    private void ResetCooldown()
    {
        _cooldown = true;
    }
}
