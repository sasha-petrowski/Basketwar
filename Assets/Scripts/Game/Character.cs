using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterMovement))]
public class Character: MonoBehaviour
{
    [HideInInspector]
    public CharacterMovement Controller;

    private void Awake()
    {
        Controller = GetComponent<CharacterMovement>();
    }
}
