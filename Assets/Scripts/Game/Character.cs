using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character: MonoBehaviour
{
    static int s_playerCount;

    public bool CanMove => CanMoveCount == 0;
    [HideInInspector]
    public int CanMoveCount;

    private void Awake()
    {
        s_playerCount++;

        GetComponent<SpriteRenderer>().color = s_playerCount % 2 == 1 ? Color.blue : Color.red;
    }
}
