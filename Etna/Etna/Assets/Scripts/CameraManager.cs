using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public Camera myCamera;
    public bool ShouldCameraRotateX;
    public bool ShouldCameraRotateY;
    public bool ShouldObjectRotateX;
    public bool ShouldObjectRotateY;
    //Maximum angle the camera can look up and down
    public int maxXRotation;
    public int maxYRotation;
    private bool useMaxXRotation = false;
    private bool useMaxYRotation = true;
    //How fast should the camera rotate left and right with mouse movement
    public float XSensitivity;
    public float YSensitivity;
    //Keeps track of the mouse X movement since the start of the game.
    float rotationX = 0;
    float rotationY = 0;
    Quaternion cameraStartRotation;
    Quaternion startRotation;
    bool shouldAlignToObject = false;

    private const float cameraBobDist = 0.15f;
    private const float cameraBobSpeed = 0.15f;
    private float currentCameraBob;
    private bool cameraBobUpOrDown;
    private Vector3 cameraStartPosition;

    private const float FOVChange = 10;
    private const float FOVChangeSpeed = 0.2f;
    private float baseFOV;
    private float targetFOV;

    // Use this for initialization
    void Start()
    {
        cameraStartPosition = myCamera.transform.localPosition;
        cameraStartRotation = myCamera.transform.localRotation;
        startRotation = transform.localRotation;
        baseFOV = Camera.main.fieldOfView;
        targetFOV = baseFOV;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameMenu_Handler.Paused)
        {
            HandleRotation();
            HandleFOV();
        }
    }

    public void Rotate(float amount)
    {
        {
            ShouldObjectRotateX = false;
            ShouldObjectRotateY = false;
            ShouldCameraRotateX = true;
            ShouldCameraRotateY = true;

            useMaxXRotation = true;

            //Should the camera rotation be originated from the object rotation. (set to true during wall running)
            shouldAlignToObject = true;

            //The desired rotation
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, amount);

            //Dont touch the 3 lines below here, they wont help you.
            startRotation = transform.localRotation;
            rotationX = 0;
            rotationY = 0;
        }
    }

    //This one reverts the character rotation and gives part of the control over the rotations back to the character, also deactivates MaxXRotation. @PIETER
    public void ResetRotation()
    {
        ShouldObjectRotateX = true;
        ShouldObjectRotateY = false;
        ShouldCameraRotateX = false;
        ShouldCameraRotateY = true;

        useMaxXRotation = false;

        shouldAlignToObject = false;

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
        startRotation = transform.localRotation;
    }

    private void HandleRotation()
    {
        //rotationX keeps track of the mouse X movement since the start of the game.
        rotationX += Input.GetAxis("Mouse X") * XSensitivity;
        rotationY += Input.GetAxis("Mouse Y") * YSensitivity;

        if (useMaxYRotation)
        {
            rotationY = Mathf.Clamp(rotationY, -maxYRotation, maxYRotation);
        }
        if (useMaxXRotation)
        {
            rotationX = Mathf.Clamp(rotationX, -maxXRotation, maxXRotation);
        }
        //Translate rotations to quaternions.
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

        Quaternion allRotations = Quaternion.Euler(xQuaternion.eulerAngles + yQuaternion.eulerAngles);

        //Set the cat rotation based on the movement of the mouse since the start of the game.
        if (ShouldObjectRotateX && ShouldObjectRotateY)
        {
            transform.localRotation = startRotation * allRotations;
        }
        else if (ShouldObjectRotateX)
        {
            transform.localRotation = startRotation * xQuaternion;
        }
        else if (ShouldObjectRotateY)
        {
            transform.localRotation = startRotation * yQuaternion;
        }
        if (ShouldCameraRotateX && ShouldCameraRotateY)
        {
            if (shouldAlignToObject)
            {
                myCamera.transform.rotation = startRotation * allRotations;
            }
            else
            {
                myCamera.transform.localRotation = cameraStartRotation * allRotations;
            }
        }
        else if (ShouldCameraRotateX)
        {
            myCamera.transform.localRotation = cameraStartRotation * xQuaternion;
        }
        else if (ShouldCameraRotateY)
        {
            myCamera.transform.localRotation = cameraStartRotation * yQuaternion;
        }
    }

    public void CameraBob()
    {
        Camera.main.transform.localPosition = cameraStartPosition + (new Vector3(0, cameraBobDist, 0) * currentCameraBob);
        if (cameraBobUpOrDown)
        {
            currentCameraBob += Time.deltaTime / cameraBobSpeed;
        }
        else
        {
            currentCameraBob -= Time.deltaTime / cameraBobSpeed;
        }
        currentCameraBob = Mathf.Clamp(currentCameraBob, 0, 1);
        if (currentCameraBob >= 1 || currentCameraBob <= 0)
        {
            cameraBobUpOrDown = !cameraBobUpOrDown;
        }
    }

    private void HandleFOV()
    {
        if (Mathf.Abs(targetFOV - Camera.main.fieldOfView) >= 1)
        {
            Camera.main.fieldOfView += (FOVChange * (Time.deltaTime / FOVChangeSpeed)) * Mathf.Sign(targetFOV - Camera.main.fieldOfView);
            Debug.Log("cam FOV: " + Camera.main.fieldOfView);
            Debug.Log("target FOV: " + targetFOV);
        }
        else
        {
            Camera.main.fieldOfView = targetFOV;
        }
    }

    public void SetFOV(float FOV)
    {
        targetFOV = FOV;
    }

    public void SetRotateValues(bool objX, bool objY, bool camX, bool camY)
    {
        ShouldObjectRotateX = objX;
        ShouldObjectRotateY = objY;
        ShouldCameraRotateX = camX;
        ShouldCameraRotateY = camY;
    }
}
