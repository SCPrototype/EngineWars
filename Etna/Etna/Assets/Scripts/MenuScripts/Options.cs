using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Startscreen");
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Startscreen");
    }

}
