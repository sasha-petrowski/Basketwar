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
public class CharacterStun: MonoBehaviour
{
    private const float BARSIZE = 5;

    #region Required Components
    [HideInInspector]
    public Character Character;
    [HideInInspector]
    public CharacterFeedback Feedback;
    private Rigidbody2D _rb;
    #endregion

    [Header("Stun")]
    public int BarRegen;

    [Header("Refs")]
    [SerializeField]
    private GameObject _stunVisual;
    [SerializeField]
    private GameObject _stunBar;

    //Logic var
    bool _stunned;
    float _maxStun;
    float _stunLevel;

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

        input.ADown += OnClick;
        #endregion
    }

    private void Update()
    {
        if(_stunned)
        {
            _stunLevel = Mathf.Clamp(_stunLevel + BarRegen * Time.deltaTime, 0, _maxStun);

            UpdateStunBar();

            if (_stunLevel == 0) LeaveStun();
        }
    }
    public void Stun(float stun)
    {
        if(TryGetComponent(out CharacterGrab grab)) grab.Drop();

        if (!_stunned)
        {
            Character.CanMoveCount++;
        }

        if(_stunLevel < stun)
        {
            _stunned = true;
            _stunVisual.SetActive(true);

            _stunLevel = stun;
            _maxStun = stun;

            UpdateStunBar();
        }
    }
    private void OnClick()
    {
        _stunLevel--;

        if (_stunLevel <= 0) LeaveStun();
    }
    public void LeaveStun()
    {
        if (_stunned) Character.CanMoveCount--;

        _stunned = false;
        _stunVisual.SetActive(false);
    }
    private void UpdateStunBar()
    {
        float width = _stunLevel <= BARSIZE ? _stunLevel / BARSIZE : (BARSIZE + Mathf.Sqrt(_stunLevel - BARSIZE)) / BARSIZE;
        _stunBar.transform.localScale = new Vector3(width, 1, 1);
    }
}
