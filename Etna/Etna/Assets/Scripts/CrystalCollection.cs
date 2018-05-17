using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalCollection : MonoBehaviour {

    public GameObject gameHandler;
    private Resources resources;

	// Use this for initialization
	void Start () {
        gameHandler = GameObject.Find("GameHandler");
        resources = gameHandler.GetComponent<Resources>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "CollectibleLight")
        {
            resources.AddEnergy(20);
            Destroy(col.gameObject);
        }
    }
}
