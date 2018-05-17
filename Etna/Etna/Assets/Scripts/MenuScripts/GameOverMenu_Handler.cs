using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu_Handler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
