using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{

    //Maximum angle the camera can look up and down
    public int cameraMaxYRotation;
    //How fast should the camera rotate left and right with mouse movement
    public float XSensitivity;
    public float YSensitivity;
    //Keeps track of the mouse X movement since the start of the game.
    float rotationX = 0;
    float rotationY = 0;
    Quaternion startRotation;
    private PlayerMovement player;

    // Use this for initialization
    void Start()
    {
        startRotation = transform.localRotation;
       

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameMenu_Handler.Paused)
        {
            HandleRotation();
        }
    }

    private void HandleRotation()
    {


        //rotationX keeps track of the mouse X movement since the start of the game.
        rotationX += Input.GetAxis("Mouse X") * XSensitivity;
        rotationY += Input.GetAxis("Mouse Y") * YSensitivity;
        rotationY = Mathf.Clamp(rotationY, -cameraMaxYRotation, cameraMaxYRotation);
        //Translate rotationX to a quaternion.
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

        Quaternion test = Quaternion.Euler(xQuaternion.eulerAngles + yQuaternion.eulerAngles);

        //Set the cat rotation based on the movement of the mouse since the start of the game.
        transform.localRotation = startRotation * test;

    }
}
