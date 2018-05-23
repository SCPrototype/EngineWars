using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitingForSure : MonoBehaviour
{

    public void BackToPause()
    {
        SceneManager.LoadScene("Pause");
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Startscreen");
    }

}
