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

    [Header("Refs")]
    [SerializeField]
    private TextMeshProUGUI _textBlue;
    [SerializeField]
    private TextMeshProUGUI _textRed;
    [SerializeField]
    private TextMeshProUGUI _textTime;

    private int _scoreBlue, _scoreRed;
    private bool _started = false;
    private float _timeLeft;


    private void Start()
    {
        StartGame();
    }
    private void Update()
    {
        if (_started)
        {
            _timeLeft -= Time.deltaTime;

            _textTime.text = Mathf.FloorToInt(_timeLeft / 60) + ":" + Mathf.FloorToInt(_timeLeft % 60);
        }
    }
    public void StartGame()
    {
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
}
