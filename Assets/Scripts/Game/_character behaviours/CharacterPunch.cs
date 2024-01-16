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
public class CharacterPunch: MonoBehaviour
{
    #region Required Components
    [HideInInspector]
    public Character Character;
    [HideInInspector]
    public CharacterFeedback Feedback;
    private Rigidbody2D _rb;
    #endregion

    [Header("Punch")]
    [SerializeField]
    private Vector2 _offset;
    public float Radius;
    [SerializeField]
    private ContactFilter2D _filter;

    [Header("Refs")]
    [SerializeField]
    private SpriteRenderer _punchSprite;

    private int _nonZeroDirection = 1;
    public Vector3 Offset => new Vector3(_offset.x * _nonZeroDirection, _offset.y, 1);

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + Offset, Radius);
    }

    private void Awake()
    {
        #region Required Components
        Character = GetComponent<Character>();
        _rb = GetComponent<Rigidbody2D>();
        Feedback = GetComponent<CharacterFeedback>();
        #endregion

        #region Subscribe to listeners
        //Inputs
        CharacterInput input = GetComponent<CharacterInput>();

        input.MoveDirection += OnDirectionChange;

        input.XDown += Punch;

        #endregion
    }

    private void OnDirectionChange(int direction)
    {
        if (direction != 0) _nonZeroDirection = direction;
    }
    private void Hit(Collider2D collider)
    {
        if (collider.TryGetComponent(out CharacterReference reference) && reference.Character != Character)
        {
            reference.Character.OnHit();
        }
    }

    private void Punch()
    {
        if (!Character.CanMove) return;

        _punchSprite.transform.localPosition = Offset;

        RaycastHit2D[] results = Physics2D.CircleCastAll(transform.position, Radius, Offset, 1, Layers.Player);

        foreach(RaycastHit2D r in results)
        {
            Hit(r.collider);
        }
    }
}
