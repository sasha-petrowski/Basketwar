using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterInput))]
[RequireComponent(typeof(CharacterFeedback))]
public class CharacterGrab : MonoBehaviour
{
    #region Required Components
    [HideInInspector]
    public Character Character;
    [HideInInspector]
    public CharacterFeedback Feedback;
    private Rigidbody2D _rb;
    #endregion

    [Header("Grab")]
    [SerializeField]
    private Vector2 _offset;
    public float Radius;
    public int StunLevel = 12;

    [Header("Throw")]
    [SerializeField]
    private Vector2 _thowForce;

    [Header("Refs")]
    [SerializeField]
    private Transform _grabTransform;

    private int _nonZeroDirection = 1;

    [HideInInspector]
    public Character Grabbed;
    public Vector3 Offset => new Vector3(_offset.x * _nonZeroDirection, _offset.y, 1);
    public Vector2 ThowForce => new Vector2(_thowForce.x * _nonZeroDirection, _thowForce.y);

    private Coroutine _animCoroutine;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Offset, Radius);
    }
    private void Update()
    {
        if(Grabbed != null) Grabbed.transform.position = _grabTransform.position;
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

        input.YDown += GrabOrThrow;

        #endregion
    }
    private void OnDirectionChange(int direction)
    {
        if (direction != 0) _nonZeroDirection = direction;
    }
    private bool TryGrab(Collider2D collider)
    {
        Debug.Log("TryGrab : " + collider.transform.parent.parent.name);
        if (collider.TryGetComponent(out CharacterReference reference) && reference.Character != Character && reference.Character.TryGetComponent(out CharacterGrab grab) && grab.Grabbed == null)
        {
            Grabbed = reference.Character;
            Grabbed.transform.SetParent(_grabTransform);
            Grabbed.transform.position = _grabTransform.position;
            Grabbed.OnGrabed(this);

            if(Grabbed.TryGetComponent(out CharacterStun stun) && stun.Stunned)
            {
                stun.Stun(StunLevel);
            }

            int catchLayer = Character.Animator.GetLayerIndex(AnimationLayers.Carry);
            Character.Animator.SetLayerWeight(catchLayer, 1);

            Debug.Log("Grabbed : " + Grabbed.name);

            return true;
        }
        return false;
    }
    private void Grab()
    {
        int grabLayer = Character.Animator.GetLayerIndex(AnimationLayers.Grab);
        Character.Animator.SetLayerWeight(grabLayer, 1);

        Character.Animator.Play(AnimationState.Grab, grabLayer, 0f);

        if (_animCoroutine != null) StopCoroutine(_animCoroutine);
        _animCoroutine = StartCoroutine(Utility.WaitFor(.5f, () => Character.Animator.SetLayerWeight(grabLayer, 0)));

        RaycastHit2D[] results = Physics2D.CircleCastAll(transform.position, Radius, Offset, 1, Layers.Player);

        foreach (RaycastHit2D r in results)
        {
            if(TryGrab(r.collider)) break;
        }
    }
    private void Throw()
    {
        int throwLayer = Character.Animator.GetLayerIndex(AnimationLayers.Throw);
        Character.Animator.SetLayerWeight(throwLayer, 1);

        Character.Animator.Play(AnimationState.Throw, throwLayer, 0.5f);

        if (_animCoroutine != null) StopCoroutine(_animCoroutine);
        _animCoroutine = StartCoroutine(Utility.WaitFor(.5f, () => Character.Animator.SetLayerWeight(throwLayer, 0)));

        Character tmp = Grabbed;
        Drop();
        tmp.GetComponent<Rigidbody2D>().velocity = _rb.velocity + ThowForce;
    }
    public void Drop()
    {
        int catchLayer = Character.Animator.GetLayerIndex(AnimationLayers.Carry);
        Character.Animator.SetLayerWeight(catchLayer, 0);

        if (Grabbed == null) return;

        Grabbed.transform.SetParent(transform.parent);
        Character tmp = Grabbed;
        Grabbed = null;
        tmp.OnDroped();
    }
    private void GrabOrThrow()
    {
        if (!Character.CanMove) return;

        if (Grabbed == null)
        {

            Debug.Log("Grab");
            Grab();
        }
        else
        {
            Debug.Log("Throw");
            Throw();
        }
    }

}
