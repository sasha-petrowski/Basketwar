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

    [Header("Throw")]
    [SerializeField]
    private Vector2 _thowForce;

    [Header("Refs")]
    [SerializeField]
    private Transform _grabTransform;

    private int _nonZeroDirection = 1;

    public Character Grabbed;
    public Vector3 Offset => new Vector3(_offset.x * _nonZeroDirection, _offset.y, 1);
    public Vector2 ThowForce => new Vector2(_thowForce.x * _nonZeroDirection, _thowForce.y);

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
        if (collider.TryGetComponent(out CharacterReference reference) && reference.Character != Character && Character.TryGetComponent(out CharacterGrab grab) && grab.Grabbed == null)
        {
            Grabbed = reference.Character;
            Grabbed.transform.SetParent(_grabTransform);
            Grabbed.transform.position = _grabTransform.position;
            Grabbed.OnGrabed(this);

            Debug.Log("Grabbed : " + Grabbed.name);

            return true;
        }
        return false;
    }
    private void Grab()
    {
        RaycastHit2D[] results = Physics2D.CircleCastAll(transform.position, Radius, Offset, 1, Layers.Player);

        foreach (RaycastHit2D r in results)
        {
            if(TryGrab(r.collider)) break;
        }
    }
    private void Throw()
    {
        Character tmp = Grabbed;
        Drop();
        tmp.GetComponent<Rigidbody2D>().velocity = _rb.velocity + ThowForce;
    }
    public void Drop()
    {
        if (Grabbed == null) return;

        Grabbed.transform.SetParent(transform.parent);
        Character tmp = Grabbed;
        Grabbed = null;
        tmp.OnDroped();
    }
    private void GrabOrThrow()
    {
        if (Grabbed == null)
        {
            if (!Character.CanMove) return;

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
