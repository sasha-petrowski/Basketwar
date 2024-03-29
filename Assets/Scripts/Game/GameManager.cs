using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    [Header("Game")]
    public int GameTime = 60;

    [Header("Refs Spawn")]
    public List<Transform> Spawnpoints;

    [Header("Refs UI")]
    [SerializeField]
    private TextMeshProUGUI _textBlue;
    [SerializeField]
    private TextMeshProUGUI _textRed;
    [SerializeField]
    private TextMeshProUGUI _textTime;
    [SerializeField]
    private TextMeshProUGUI _textJoin;

    [Header("Refs Win")]
    [SerializeField]
    private GameObject _winGO;
    [SerializeField]
    private TextMeshProUGUI _textWin;

    private int _scoreBlue, _scoreRed;
    private bool _started = false;
    private bool _finished = false;
    private float _timeLeft;
    public bool Overtime { get; private set; } = false;

    private List<Character> _characterList = new List<Character>();

    private void Update()
    {
        if (_started & !_finished)
        {
            _timeLeft -= Time.deltaTime;

            if(Overtime)
            {
                _textTime.text = "Overtime : " + Mathf.FloorToInt(-_timeLeft / 60) + ":" + Mathf.FloorToInt(-_timeLeft % 60);
            }
            else
            {
                _textTime.text = Mathf.FloorToInt(_timeLeft / 60) + ":" + Mathf.FloorToInt(_timeLeft % 60);
            }

            if(_timeLeft <= 0 && ! Overtime)
            {
                TryEndGame();
            }
        }
    }
    private void TryEndGame()
    {
        if(_scoreBlue != _scoreRed)
        {
            WinGame(_scoreBlue > _scoreRed ? Team.Blue : Team.Red);
        }
        else
        {
            Overtime = true;
        }
    }
    public void WinGame(Team team)
    {
        _finished = true;

        _winGO.SetActive(true);

        _textWin.text = team.ToString();

        _textWin.color = team == Team.Blue ? Color.blue : Color.red;
    }
    public void StartGame()
    {
        foreach(Character character in _characterList)
        {
            character.CanMoveCount--;
        }
        _textJoin.gameObject.SetActive(false);

        _started = true;
        _timeLeft = GameTime;
    }
    public void AddScore(int score, Team team)
    {
        switch(team)
        {
            case Team.Blue:
                _scoreBlue += score;
                _textBlue.text = _scoreBlue.ToString();
                break;

            case Team.Red:
                _scoreRed += score;
                _textRed.text = _scoreRed.ToString();
                break;
        }
    }

    public void AddPlayer(Character character)
    {
        _characterList.Add(character);
        character.CanMoveCount++;
        if (_characterList.Count == 4)
        {
            StartGame();
        }
    }
}
