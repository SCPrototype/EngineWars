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
    public float FallingSpeedOnWall;
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

    private BoxCollider interactCollider;
    private Vector3 velocityHolder;
    private Vector3 vaultStart;
    private Vector3 vaultTarget;

    private enum MovementState
    {
        Idle,
        Walk,
        Run,
        Jump,
        Fall,
        Vault,
        Slide,
        WallRun,
        WallCling
    }

    private MovementState myMovementState = MovementState.Idle;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dynamicLines = GetComponent<ParticleSystem>();
        baseFOV = Camera.main.fieldOfView;
        feetCollider = GameObject.Find("FeetCollider").GetComponent<BoxCollider>();
        rightCollider = GameObject.Find("SideColliderRight").GetComponent<BoxCollider>();
        leftCollider = GameObject.Find("SideColliderLeft").GetComponent<BoxCollider>();
        interactCollider = GameObject.Find("InteractCollider").GetComponent<BoxCollider>(); ;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameMenu_Handler.Paused)
        {
            //if (rb.isKinematic)
            //{
            //    rb.isKinematic = false;
            //}
            HandleInput();
            HandleGrounded();
            HandleWall();
            HandleState();
        }
        //else if (!rb.isKinematic)
        //{
        //    rb.isKinematic = true;
        //}
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
                rb.useGravity = true;
                myMovementState = MovementState.Walk;
                break;
            }
        }

        
    }

    private void HandleWall()
    {
        isOnWall = false;
        rb.useGravity = true;
        Collider[] allOverlappingCollidersLeft = Physics.OverlapBox(rightCollider.bounds.center, rightCollider.bounds.extents);

        foreach (Collider collidedObject in allOverlappingCollidersLeft)
        {
            if (collidedObject.tag == "Wall" && isGrounded == false)
            {
                isOnWall = true;
                rb.useGravity = false;
                myMovementState = MovementState.WallRun;
                break;
            }
        }
        Collider[] allOverlappingCollidersRight = Physics.OverlapBox(leftCollider.bounds.center, leftCollider.bounds.extents);
        foreach (Collider collidedObject in allOverlappingCollidersRight)
        {
            if (collidedObject.tag == "Wall" && isGrounded == false)
            {
                isOnWall = true;
                rb.useGravity = false;
                myMovementState = MovementState.WallRun;
                break;
            }
        }
    }

    private void HandleInput()
    {
        if (isGrounded)
        {
            if (myMovementState != MovementState.Vault && myMovementState != MovementState.Slide)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    rb.velocity += transform.forward * MovementSpeed * Time.deltaTime;
                    //rb.AddForce(transform.forward * MovementSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    //rb.AddForce(-transform.right * MovementSpeed * Time.deltaTime);
                    rb.velocity += -transform.right * MovementSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    //rb.AddForce(-transform.forward * MovementSpeed * Time.deltaTime);
                    rb.velocity += -transform.forward * MovementSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    //rb.AddForce(transform.right * MovementSpeed * Time.deltaTime);
                    rb.velocity += transform.right * MovementSpeed * Time.deltaTime;
                }
                if (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude >= MaxSpeed)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * MaxSpeed + new Vector3(0, rb.velocity.y, 0);
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!checkInteractable(KeyCode.Space))
                    {
                        Jump();
                    }
                }
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    if (!checkInteractable(KeyCode.LeftShift))
                    {
                        //Jump();
                    }
                }
            }
        }
        else
        {
            if(myMovementState == MovementState.WallRun)
            {

            }
            rb.drag = 0.1f;
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

    private void HandleState()
    {
        switch (myMovementState)
        {
            case MovementState.Idle:
                break;
            case MovementState.Walk:
                break;
            case MovementState.Run:
                break;
            case MovementState.Jump:
                break;
            case MovementState.Fall:
                break;
            case MovementState.Vault:
                Debug.Log("vaulting");
                transform.position += (vaultTarget - vaultStart) * Time.deltaTime * (0.5f + (velocityHolder.magnitude / 4));
                if (Vector3.Distance(transform.position, vaultStart) >= Vector3.Distance(vaultTarget, vaultStart))
                {
                    rb.isKinematic = false;
                    rb.velocity = velocityHolder;
                    transform.position += new Vector3(0, -1, 0);
                    if (velocityHolder.magnitude > 0)
                    {
                        myMovementState = MovementState.Run;
                    }
                    else
                    {
                        myMovementState = MovementState.Idle;
                    }
                }
                break;
            case MovementState.Slide:
                Debug.Log("sliding");
                transform.position += (vaultTarget - vaultStart) * Time.deltaTime * (0.5f + (velocityHolder.magnitude / 4));
                if (Vector3.Distance(transform.position, vaultStart) >= Vector3.Distance(vaultTarget, vaultStart))
                {
                    rb.isKinematic = false;
                    rb.velocity = velocityHolder;
                    transform.position += new Vector3(0, 1.5f, 0);
                    if (velocityHolder.magnitude > 0)
                    {
                        myMovementState = MovementState.Run;
                    }
                    else
                    {
                        myMovementState = MovementState.Idle;
                    }
                }
                break;
            case MovementState.WallRun:
                
                rb.AddForce(transform.forward * MovementSpeed * Time.deltaTime);

                if (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude >= MaxSpeed)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * MaxSpeed + new Vector3(0, rb.velocity.y, 0);
                }
                rb.AddForce(-transform.up * FallingSpeedOnWall * Time.deltaTime);
                Debug.Log(rb.velocity.y);
                break;
            case MovementState.WallCling:
                break;
            default:
                break;
        }
    }

    private bool checkInteractable(KeyCode key)
    {
        Collider[] allOverlappingColliders = Physics.OverlapBox(interactCollider.bounds.center, interactCollider.bounds.extents);

        foreach (Collider collidedObject in allOverlappingColliders)
        {
            if (collidedObject.GetComponent<Interactable>() != null)
            {
                if (collidedObject.GetComponent<Interactable>().GetInteractKey() == key)
                {
                    collidedObject.GetComponent<Interactable>().Interact(this);
                    return true;
                }
            }
        }
        return false;
    }

    public Rigidbody GetRigidBody()
    {
        return rb;
    }

    public void Vault(Vector3 start, Vector3 end)
    {
        velocityHolder = rb.velocity;
        rb.isKinematic = true;
        transform.position += new Vector3(0, 1, 0);
        vaultStart = transform.position;
        vaultTarget = transform.position + ((end - start).normalized * Vector3.Distance(transform.position, end));
        //transform.position += end - start;
        myMovementState = MovementState.Vault;
    }

    public void Slide(Vector3 start, Vector3 end)
    {
        velocityHolder = rb.velocity;
        rb.isKinematic = true;
        transform.position += new Vector3(0, -1.5f, 0);
        vaultStart = transform.position;
        vaultTarget = transform.position + ((end - start).normalized * Vector3.Distance(transform.position, end));
        myMovementState = MovementState.Slide;
    }

    public void Jump()
    {
        myMovementState = MovementState.Jump;
        rb.AddForce(transform.up * JumpHeight);
    }

    public void WallCling()
    {
        myMovementState = MovementState.WallCling;
        Debug.Log("WallCling");
        rb.velocity -= transform.up * FallingSpeedOnWall * Time.deltaTime;
    }
}
