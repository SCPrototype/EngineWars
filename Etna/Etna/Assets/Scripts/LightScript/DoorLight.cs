using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLight : MonoBehaviour {

    public GameObject lightPrefab;
    private List<Transform> doorsInRoom;
    // Use this for initialization
    void Start () {
        doorsInRoom = GetDoorsInRoom();
        foreach (Transform doorpost in doorsInRoom)
        {
            GameObject doorLight = Instantiate(lightPrefab, transform);
            doorLight.transform.LookAt(doorpost);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private List<Transform> GetDoorsInRoom()
    {
        List<Transform> doorList = new List<Transform>();

        //Get the current box you're in.
        Transform currentBox = this.gameObject.transform.root;

        Transform[] boxChildList = currentBox.GetComponentsInChildren<Transform>();
        //Get all wall objects in the box.
        foreach (Transform DoorPost in boxChildList)
        {
            if (DoorPost.tag == "HasDoor")
            {
                doorList.Add(DoorPost);
            }
        }

        return doorList;
    }
}
