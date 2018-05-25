using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	void Update ()
    {

	}

    public void ResumeGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ShowOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("QuitingForSure");
    }
}
