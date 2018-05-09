using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(ParticleSystem))]
public class PlayerMovement : MonoBehaviour
{

    public float MovementSpeed;
    public float MovementSpeedInAir;
    public float MaxSpeed;
    public float MaxSpeedInAir;
    public float JumpHeight;
    private bool isGrounded;
    private bool isOnWall;
    private Rigidbody rb;

    private const float FOVChange = 25;
    private float baseFOV;
    private ParticleSystem dynamicLines;
    private BoxCollider feetCollider;
    private BoxCollider rightCollider;
    private BoxCollider leftCollider;
    private GameObject rightBox;
    private GameObject leftBox;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dynamicLines = GetComponent<ParticleSystem>();
        baseFOV = Camera.main.fieldOfView;
        feetCollider = GetComponent<BoxCollider>();
        rightBox = GameObject.Find("SideColliderRight");
        rightCollider = rightBox.GetComponent<BoxCollider>();
        leftBox = GameObject.Find("SideColliderLeft");
        leftCollider = leftBox.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleGrounded();
        HandleWall();
    }

    private void HandleGrounded()
    {
        isGrounded = false;
        Collider[] allOverlappingColliders = Physics.OverlapBox(feetCollider.bounds.center, feetCollider.bounds.extents);

        foreach (Collider collidedObject in allOverlappingColliders)
        {
            if (collidedObject.tag == "Ground")
            {
                isGrounded = true;
                break;
            }
        }

        
    }

    private void HandleWall()
    {
        isOnWall = false;
        Collider[] allOverlappingCollidersLeft = Physics.OverlapBox(rightCollider.bounds.center, rightCollider.bounds.extents);

        foreach (Collider collidedObject in allOverlappingCollidersLeft)
        {
            if (collidedObject.tag == "Wall")
            {
                Debug.Log("Character is on a wall left");
                isOnWall = true;
                break;
            }
        }

        Collider[] allOverlappingCollidersRight = Physics.OverlapBox(leftCollider.bounds.center, leftCollider.bounds.extents);

        foreach (Collider collidedObject in allOverlappingCollidersRight)
        {
            if (collidedObject.tag == "Wall")
            {
                Debug.Log("Character is on a wall right");
                isOnWall = true;
                break;
            }
        }
    }

    private void HandleInput()
    {
        if (isOnWall)
        {
            rb.useGravity = false;
            isGrounded = true;
        }
        else
        {
            rb.useGravity = true;
        }
        if (isGrounded)
        {

            rb.drag = 3;
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(transform.forward * MovementSpeed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(-transform.right * MovementSpeed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(-transform.forward * MovementSpeed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(transform.right * MovementSpeed);
            }
            if (rb.velocity.magnitude >= MaxSpeed)
            {
                rb.velocity = rb.velocity.normalized * MaxSpeed;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(transform.up * JumpHeight);
            }
        }
        else
        {
            rb.drag = 1;
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(transform.forward * MovementSpeedInAir);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(-transform.right * MovementSpeedInAir);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(-transform.forward * MovementSpeedInAir);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(transform.right * MovementSpeedInAir);
            }
            if (rb.velocity.magnitude >= MaxSpeed)
            {
                rb.velocity = rb.velocity.normalized * MaxSpeedInAir;
            }
        }

        //if (rb.velocity.magnitude >= 0.3f)
        //{
        //    Camera.main.fieldOfView = baseFOV + (FOVChange * (rb.velocity.magnitude / MaxSpeed));
        //    dynamicLines.Play();
        //}
        //else
        //{
        //    dynamicLines.Stop();
        //    Camera.main.fieldOfView = baseFOV;
        //}
    }
}
