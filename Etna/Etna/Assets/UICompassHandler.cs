using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICompassHandler : MonoBehaviour
{

    private List<Transform> doorsInRoom;
    // Use this for initialization
    void Start()
    {
        doorsInRoom = GetDoorsInRoom();
        foreach (Transform doorpost in doorsInRoom)
        {
            Debug.Log(doorpost.name + " " + doorpost.position);
        }

    }

    private List<Transform> GetDoorsInRoom()
    {
        List<Transform> doorList = new List<Transform>();

        //Get the current box you're in.
        GameObject currentBox = GameObject.Find("box");
        Transform[] boxChildList = currentBox.transform.GetComponentsInChildren<Transform>();
        //Get all wall objects in the box.
        foreach (Transform DoorPost in boxChildList)
        {
            if (DoorPost.name == "Doorpost")
            {
                doorList.Add(DoorPost);
            }
        }

        return doorList;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
