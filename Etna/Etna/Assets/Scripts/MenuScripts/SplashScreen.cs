using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKey)
        {
            GoToMainMenu();
        }
    }

    public void GoToMainMenu()
    {
        //print("Go to Startscreen");
        SceneManager.LoadScene("AllMenuScenes");
    }
}
