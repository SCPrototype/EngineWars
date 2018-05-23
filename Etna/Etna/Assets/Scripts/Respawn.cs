using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class Respawn : MonoBehaviour {

    private Vector3 respawnPos;

	// Use this for initialization
	void Start () {
        respawnPos = transform.position;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = respawnPos;
        }
	}

    public void SetRespawnPos(Vector3 newPos)
    {
        respawnPos = newPos;
    }
}
