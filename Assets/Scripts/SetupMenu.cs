using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupMenu : MonoBehaviour
{
    private int _PlayerIndex;
    [SerializeField]
    private TextMeshProUGUI titletext;
    [SerializeField]
    private GameObject readypanel;
    [SerializeField]
    private GameObject readyButton;

    private float ignoreInputTime = 1.5f;
    private bool inputEnabled;

    public void SetPlayerIndex(int pi)
    {
        _PlayerIndex = pi;
        titletext.SetText("PLAYER" + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }
}
