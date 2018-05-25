using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuHandler mh = GameObject.FindGameObjectWithTag("UIManager").GetComponent<MenuHandler>();
            mh.ActivateStartScreen();
            //SceneManager.LoadScene("Startscreen");
        }
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Startscreen");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("LevelTutorial");
    }
}
