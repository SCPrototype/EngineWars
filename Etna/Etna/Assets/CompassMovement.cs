using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassMovement : MonoBehaviour {

    private GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        if(null == player)
        {
            Debug.Log("Player not found in CompassMovement");
        }
	}
	
	// Update is called once per frame
	void Update () {
        float centerX = Mathf.Lerp(500, 750, (float)(player.transform.eulerAngles.y / 360 + 0.5) % 1);
        this.transform.position = new Vector3(centerX, Screen.height - 70, 0);
    }
}
