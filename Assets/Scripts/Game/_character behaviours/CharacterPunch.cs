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

    private int _nonZeroDirection = 1;
    public Vector3 Offset => new Vector3(_offset.x * _nonZeroDirection, _offset.y, 1);

    Coroutine _animCoroutine;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
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

        int punchLayer = Character.Animator.GetLayerIndex(AnimationLayers.Bonk);
        Character.Animator.SetLayerWeight(punchLayer, 1);

        Character.Animator.Play(AnimationState.Bonk, punchLayer, 0.4f);

        if(_animCoroutine != null) StopCoroutine(_animCoroutine);
        _animCoroutine = StartCoroutine(Utility.WaitFor(.5f, () => Character.Animator.SetLayerWeight(punchLayer, 0)));

        RaycastHit2D[] results = Physics2D.CircleCastAll(transform.position, Radius, Offset, 1, Layers.Player);

        foreach(RaycastHit2D r in results)
        {
            Hit(r.collider);
        }
    }
}
