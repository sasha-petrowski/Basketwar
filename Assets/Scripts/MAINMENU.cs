using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MAINMENU : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MENU;
    public Button Quit;
    public Button Play;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        MENU.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
