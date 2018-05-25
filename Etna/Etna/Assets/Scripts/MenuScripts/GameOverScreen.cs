using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour {

    void Start()
    {
        Cursor.visible = true;
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitTheGame()
    {
        SceneManager.LoadScene("AllMenuScenes");
    }
}
