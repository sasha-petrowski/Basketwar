using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Character: MonoBehaviour
{
    #region Required Components
    private Rigidbody2D _rb;
    #endregion

    static int s_playerCount;

    [Header("Health")]
    [SerializeField]
    private int _maxHealth = 10;

    [Header("KO")]
    [SerializeField]
    private int _stunKO = 6;
    
    [Header("Refs")]
    [SerializeField]
    private Collider2D _collider;
    [SerializeField]
    private TextMeshPro _playerTag;

    public List<GameObject> Models;

    [HideInInspector]
    public Animator Animator;

    private int _health;

    public bool CanMove => CanMoveCount == 0;
    [HideInInspector]
    public Transform SpawnPoint;
    [HideInInspector]
    public int CanMoveCount;
    [HideInInspector]
    public CharacterGrab GrabbedBy;
    [HideInInspector]
    public Team Team;
    private int _id;

    private void Awake()
    {
        #region Required Components
        _rb = GetComponent<Rigidbody2D>();
        #endregion

        s_playerCount++;
        _id = s_playerCount;

        name = this.GetType().Name + " " + s_playerCount;

        _health = _maxHealth;

        Team = s_playerCount % 2 == 1 ? Team.Blue : Team.Red;

        _playerTag.text = "P." + s_playerCount;
        _playerTag.color = s_playerCount % 2 == 1 ? Color.blue : Color.red;

        GameObject model = GameObject.Instantiate(Models[_id - 1], transform);
        model.transform.localPosition = new Vector3(0, -1, 0);
        model.transform.rotation = Quaternion.Euler(0, 120, 0);
        model.transform.localScale = Vector3.one * 2.25f;

        Animator = model.GetComponent<Animator>();
        GetComponent<CharacterMovement>().RotateModel = model.transform;
    }

    private void Start()
    {
        GameManager.Instance.AddPlayer(this);
        SpawnPoint = GameManager.Instance.Spawnpoints[_id-1];
        transform.position = SpawnPoint.position;
    }

    public void OnHit()
    {
        if (TryGetComponent(out CharacterStun stun) && stun.Stunned) return; // Don't take damage when stunned

        _health -= 1;

        if(_health == 0)
        {
            _health = _maxHealth;

            stun?.Stun(_stunKO);
        }
    }

    public void OnGrabed(CharacterGrab graber)
    {
        GrabbedBy = graber;

        _collider.isTrigger = true;
    }
    public void OnDroped()
    {
        if (GrabbedBy == null) return;

        CharacterGrab tmp = GrabbedBy;
        GrabbedBy = null;
        tmp.Drop();

        _rb.gravityScale = 1;
        // wait a tiny bit not to collide with thrower
        StartCoroutine(Utility.WaitFor(0.05f, () => _collider.isTrigger = false));
    }

    public void OnGoal()
    {
        if (TryGetComponent(out CharacterStun stun))
        {
            stun.EndStun();
        }
        if (TryGetComponent(out CharacterGrab grab))
        {
            grab.Drop();
        }
        transform.position = SpawnPoint.position;
        _rb.velocity = Vector3.zero;
    }
}
