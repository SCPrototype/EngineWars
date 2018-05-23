using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startscreen : MonoBehaviour
{
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("Escape is pressed");
            SceneManager.LoadScene("SplashScreen");
        }
	}

    public void StartGame()
    {
        SceneManager.LoadScene("LoadGame");
    }

    public void StartOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void StartCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitTheGame()
    {
        SceneManager.LoadScene("QuitingForGame");
    }
}
