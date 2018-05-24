using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICompassHandler : MonoBehaviour
{
    public GameObject DoorIcon;
    public GameObject CompassPanel;
    private List<Transform> doorsInRoom;
    private List<RectTransform> iconsInRoom = new List<RectTransform>();
    private GameObject player;
    private Vector3 northVector = new Vector3(0, 0, 1);

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        doorsInRoom = GetDoorsInRoom();
        if (null != doorsInRoom)
        {
            foreach (Transform doorpost in doorsInRoom)
            {
                Debug.Log(doorpost.name + " " + doorpost.position);
            }
            CreateIcons();
        } else
        {
            Debug.Log("box not found");
        }

    }

    private List<Transform> GetDoorsInRoom()
    {
        List<Transform> doorList = new List<Transform>();

        //Get the current box you're in.
        GameObject currentBox = GameObject.Find("box");
        if (null != currentBox)
        {
            Transform[] boxChildList = currentBox.transform.GetComponentsInChildren<Transform>();
            //Get all wall objects in the box.
            foreach (Transform DoorPost in boxChildList)
            {
                if (DoorPost.tag == "HasDoor")
                {
                    doorList.Add(DoorPost);
                }
            }

            return doorList;
        } else
        {
            return null;
        }
    }

    private void CreateIcons()
    {
        foreach (Transform door in doorsInRoom)
        {
            GameObject localGameObject = Instantiate(DoorIcon, transform);
            localGameObject.transform.SetParent(CompassPanel.transform);
            RectTransform rectTransform = localGameObject.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0, 0, 0);
            iconsInRoom.Add(rectTransform);
        }
    }

    private void HandleIconMovement()
    {
        int count = 0;
        foreach (RectTransform icon in iconsInRoom)
        {
            /*Vector van speler naar noord -> 0,0,1
            Vector van speler naar door -> ex. *.*.*
            Degree difference in vectors. -> %
            % van 360*, move it in a direction from north. */
            Vector3 PlayerToDoor = player.transform.localEulerAngles - doorsInRoom[count].position;
            PlayerToDoor.Normalize();
            PlayerToDoor.y = 0;
            Debug.DrawLine(player.transform.position, doorsInRoom[count].position);
            Debug.DrawRay(player.transform.position, northVector, Color.blue, 20);
            float angle = Vector3.Angle(northVector, PlayerToDoor);
            Debug.Log(angle);
            // Debug.Log("Angle is: " + angle);
            float widthOfBar = (580) / 2;
            ////Get the angle of position between player and the door.
            //var angle = Vector3.Angle(player.transform.localEulerAngles, doorsInRoom[count].position);
            ////float combinedAngle = positionToObject.x * positionToObject.z;
            float centerX = Mathf.Lerp(widthOfBar, Screen.width - widthOfBar, (float)(player.transform.localEulerAngles.y / 360 + 0.5) % 1);

            icon.localPosition = new Vector3(centerX, 0, 0);
            count++;
        }
        //angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
    }

    // Update is called once per frame
    void Update()
    {
        HandleIconMovement();
    }
}
