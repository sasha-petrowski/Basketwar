using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
    private SpriteRenderer _healthBar;
    [SerializeField]
    private Gradient _healthColor;

    [Header("KO")]
    [SerializeField]
    private int _stunKO = 6;

    [Header("Grab")]
    [SerializeField]
    private Collider2D _collider;


    private int _health;

    public bool CanMove => CanMoveCount == 0;
    [HideInInspector]
    public int CanMoveCount;

    public CharacterGrab GrabbedBy;


    private void Awake()
    {
        #region Required Components
        _rb = GetComponent<Rigidbody2D>();
        #endregion

        s_playerCount++;

        name = this.GetType().Name + " " + s_playerCount;

        _health = _maxHealth;

        GetComponent<SpriteRenderer>().color = s_playerCount % 2 == 1 ? Color.blue : Color.red;
    }
    public void OnHit()
    {
        _health -= 1;

        if(_health == 0)
        {
            _health = _maxHealth;

            if(TryGetComponent(out CharacterStun stun))
            {
                stun.Stun(_stunKO);
            }

        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float width = _health / (float)_maxHealth;
        _healthBar.transform.localScale = new Vector3(width, 1, 1);
        _healthBar.color = _healthColor.Evaluate(width);
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

        // wait a tiny bit not to collide with thrower
        StartCoroutine(Utility.WaitFor(0.05f, () => _collider.isTrigger = false));
    }
}
