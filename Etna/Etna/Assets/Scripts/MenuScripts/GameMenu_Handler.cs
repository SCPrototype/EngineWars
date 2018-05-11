using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu_Handler : MonoBehaviour
{
    public GameObject PauseMenu;
    public static bool Paused = false;

    // Use this for initialization
    void Start()
    {
        PauseMenu.SetActive(false);
        Paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu(!PauseMenu.activeSelf);
        }
    }

    public void TogglePauseMenu(bool toggle)
    {
        PauseMenu.SetActive(toggle);
        Paused = toggle;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("LevelOne");
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
