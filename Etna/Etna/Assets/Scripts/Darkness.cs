using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkness : MonoBehaviour {

    private GameManager myGameManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ActivationLight>() != null)
        {
            if (other.GetComponent<ActivationLight>().GetIsActivated())
            {
                myGameManager.SlowDownDarkness();
            }
        }
        else if (other.tag == "Player")
        {
            myGameManager.SetGameOver();
        }
    }

    public void SetGameManager(GameManager gm)
    {
        myGameManager = gm;
    }
}
