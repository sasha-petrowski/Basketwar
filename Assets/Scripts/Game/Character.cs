using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character: MonoBehaviour
{
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

    private int _health;


    public bool CanMove => CanMoveCount == 0;
    [HideInInspector]
    public int CanMoveCount;


    private void Awake()
    {
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
}
