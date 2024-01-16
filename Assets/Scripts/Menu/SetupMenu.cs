using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SetupMenu : MonoBehaviour
{
    private int PlayerIndex;
    [SerializeField]
    private TextMeshProUGUI titletext;
    [SerializeField]
    private GameObject readypanel;
    [SerializeField]
    private GameObject readyButton;
    [SerializeField]
    private GameObject MainLayout;
    [SerializeField]
    private GameObject CharacterPrefab;
    [SerializeField]
    private GameObject PlayerInput;


    public PlayerConfig playerconfig;
    public static int Readyplayer;

    private bool _ready = false;
    public void SetPlayerIndex(int pi)
    {
        PlayerIndex = PlayerConfig.PlayerCount;
        titletext.SetText("PLAYER " + PlayerConfig.PlayerCount);
    }

    // Update is called once per frame
    void Update()
    {
        if (Readyplayer == 4)
        {
            MainLayout.gameObject.SetActive(false);
        }
    }

    public void ReadyPlayer()
    {
        if (_ready) return;

        _ready = true;

        readyButton.gameObject.SetActive(false);
        Readyplayer++;
        //Instantiate(CharacterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        PlayerInput.gameObject.SetActive(true);
    }

    
}
