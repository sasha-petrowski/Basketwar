using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class DEBUG_TemporaryInputs : MonoBehaviour
{
    private void Awake()
    {
        Debug.LogWarning($"Using temporary script : TemporaryInputs in \"{gameObject}\"");
    }

    public CharacterMovement Controller;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");


        Controller.InputX(horizontal);
        Controller.InputY(vertical);
    }
}
