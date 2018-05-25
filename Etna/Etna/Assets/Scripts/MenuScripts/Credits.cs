using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
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

    public void BackToMenu()
    {
        SceneManager.LoadScene("Startscreen");
    }
}
