using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfig : MonoBehaviour
{
    [SerializeField]
    private RectTransform ui;
    private List<PlayerConfiguration> playerConfigs = new List<PlayerConfiguration>();

    [SerializeField]
    private int MaxPlayers = 4;

    public static int PlayerCount = 0;

    public static PlayerConfig Instance { get; private set; }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;
        if (playerConfigs.Count == MaxPlayers && playerConfigs.All( p => p.IsReady == true))
        {
            Debug.Log("pret");
        }
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        PlayerCount++;
        if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            pi.transform.SetParent(ui);
            playerConfigs.Add(new PlayerConfiguration(pi));
        }
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
}
