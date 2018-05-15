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
    public float JumpHeight;
    private bool isGrounded;
    private bool isOnWall;
    private bool isCameraTilted = false;
    public float FallingSpeedOnWall;

    private Rigidbody rb;
    private const float groundDrag = 8f;
    private const float airDrag = 0.1f;

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

    private Camera playerCamera;
    private Vector3 playerMovementOnWall;

    public enum MovementState
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

    private enum WallSide
    {
        Left,
        Right
    }

    private MovementState myMovementState = MovementState.Idle;
    private WallSide myWallSide;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dynamicLines = GetComponent<ParticleSystem>();
        baseFOV = Camera.main.fieldOfView;
        feetCollider = GameObject.Find("FeetCollider").GetComponent<BoxCollider>();
        rightCollider = GameObject.Find("SideColliderRight").GetComponent<BoxCollider>();
        leftCollider = GameObject.Find("SideColliderLeft").GetComponent<BoxCollider>();
        interactCollider = GameObject.Find("InteractCollider").GetComponent<BoxCollider>();
        playerCamera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameMenu_Handler.Paused)
        {
            if (rb.isKinematic && myMovementState != MovementState.Vault && myMovementState != MovementState.Slide)
            {
                rb.isKinematic = false;
            }
            HandleInput();
            HandleGrounded();
            HandleWall();
            HandleState();
        }
        else if (!rb.isKinematic)
        {
            rb.isKinematic = true;
        }
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
                myMovementState = MovementState.Run;
                //Reset camera.
                if(isCameraTilted == true)
                {
                    isCameraTilted = false;
                    playerCamera.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
                
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
                myMovementState = MovementState.WallRun;
                myWallSide = WallSide.Left;
                //if (isCameraTilted == false)
                //{
                //    playerCamera.transform.Rotate(new Vector3(0, 0, 20));
                //    isCameraTilted = true;
                //}
                break;
            }
        }

        Collider[] allOverlappingCollidersRight = Physics.OverlapBox(leftCollider.bounds.center, leftCollider.bounds.extents);
        foreach (Collider collidedObject in allOverlappingCollidersRight)
        {
            if (collidedObject.tag == "Wall" && isGrounded == false)
            {
                isOnWall = true;
                myMovementState = MovementState.WallRun;
                myWallSide = WallSide.Right;
                //if (isCameraTilted == false)
                //{
                //    playerCamera.transform.Rotate(new Vector3(0, 0, -20));
                //    isCameraTilted = true;
                //}
                break;
            }
        }

        if(isOnWall == false)
        {
            myMovementState = MovementState.Fall;
        }
    }

    private void HandleInput()
    {
        /*if (isOnWall)
        {
            rb.useGravity = false;
            isGrounded = true;
        }
        else
        {
            rb.useGravity = true;
        }*/
        if(myMovementState == MovementState.WallRun)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(myWallSide == WallSide.Right)
                { rb.AddForce(transform.right * 900); } else
                { rb.AddForce(-transform.right * 900); }  
            }
        }
        if (isGrounded)
        {
            if (myMovementState != MovementState.Vault && myMovementState != MovementState.Slide)
            {
                rb.drag = groundDrag;
                if (Input.GetKey(KeyCode.W))
                {
                    //rb.velocity += transform.forward * MovementSpeed * Time.deltaTime;
                    rb.AddForce(transform.forward * MovementSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    rb.AddForce(-transform.right * MovementSpeed * Time.deltaTime);
                    //rb.velocity += -transform.right * MovementSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    rb.AddForce(-transform.forward * MovementSpeed * Time.deltaTime);
                    //rb.velocity += -transform.forward * MovementSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    rb.AddForce(transform.right * MovementSpeed * Time.deltaTime);
                    //rb.velocity += transform.right * MovementSpeed * Time.deltaTime;
                }
                if (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude >= MaxSpeed)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * MaxSpeed + new Vector3(0, rb.velocity.y, 0);
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    checkInteractable(KeyCode.Space);
                    if ((myMovementState == MovementState.Idle || myMovementState == MovementState.Walk || myMovementState == MovementState.Run) && isGrounded)
                    {
                        Jump();
                    }
                }
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    checkInteractable(KeyCode.LeftShift);
                    /*if ((myMovementState == MovementState.Idle || myMovementState == MovementState.Walk || myMovementState == MovementState.Run) && isGrounded)
                    {
                        //Jump();
                    }*/
                }
            }
        }
        else
        {
            
            rb.drag = airDrag;
            if (Input.GetKey(KeyCode.W))
            {
                //rb.velocity += transform.forward * MovementSpeed * Time.deltaTime * 0.003f;
                rb.AddForce(transform.forward * MovementSpeedInAir * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(-transform.right * MovementSpeedInAir * Time.deltaTime);
                //rb.velocity += -transform.right * MovementSpeed * Time.deltaTime * 0.003f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(-transform.forward * MovementSpeedInAir * Time.deltaTime);
                //rb.velocity += -transform.forward * MovementSpeed * Time.deltaTime * 0.003f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(transform.right * MovementSpeedInAir * Time.deltaTime);
                //rb.velocity += transform.right * MovementSpeed * Time.deltaTime * 0.003f;
            }
            if (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude >= MaxSpeed)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * MaxSpeed + new Vector3(0, rb.velocity.y, 0);
            }
        }
        //Debug.Log(new Vector2(rb.velocity.x, rb.velocity.z).magnitude);
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
                //Debug.Log("idling");
                if (new Vector2(rb.velocity.x, rb.velocity.z).magnitude > 0)
                {
                    myMovementState = MovementState.Run;
                }
                break;
            case MovementState.Run:
                //Debug.Log("running");
                if (new Vector2(rb.velocity.x, rb.velocity.z).magnitude == 0)
                {
                    myMovementState = MovementState.Idle;
                }
                break;
            case MovementState.Jump:
                //Debug.Log("jumping");
                if (rb.velocity.y <= 0)
                {
                    myMovementState = MovementState.Fall;
                }
                break;
            case MovementState.Fall:
                //Debug.Log("falling");
                if (isGrounded)
                {
                    if (new Vector2(rb.velocity.x, rb.velocity.z).magnitude > 0)
                    {
                        myMovementState = MovementState.Run;
                    }
                    else
                    {
                        myMovementState = MovementState.Idle;
                    }
                }
                break;
            case MovementState.Vault:
                //Debug.Log("vaulting");
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
                //Debug.Log("sliding");
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
                rb.AddForce(transform.up * 5);
                //Debug.Log(rb.velocity.y);
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

    public MovementState GetMovementState()
    {
        return myMovementState;
    }
}
