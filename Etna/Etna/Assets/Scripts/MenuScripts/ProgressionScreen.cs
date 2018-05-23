using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressionScreen : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("Startscreen");
    }

    public void NextLevel()
    {
        //SceneManager.LoadScene("Game");
    }
}
