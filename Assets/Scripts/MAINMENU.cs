using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MAINMENU : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MENU;
    public Button Quit;
    public Button Play;
    public string Final;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(Final);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
