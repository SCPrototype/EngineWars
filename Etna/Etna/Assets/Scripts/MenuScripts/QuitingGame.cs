using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitingGame : MonoBehaviour
{
    public void BackToPause()
    {
        //SceneManager.LoadScene("Quit");
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("AllMenuScenes");
    }
}
