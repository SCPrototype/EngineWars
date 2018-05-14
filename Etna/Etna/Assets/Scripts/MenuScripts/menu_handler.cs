using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu_handler : MonoBehaviour {

    // Use this for initialization
    void Start () {
        Debug.Log("Loaded scene");
        //SceneManager.LoadScene("LevelOne", LoadSceneMode.Single);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadLevel(int indx)
    {
        SceneManager.LoadScene(indx);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
