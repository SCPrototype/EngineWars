using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float MovementSpeed;
    public float MovementSpeedInAir;
    public float MaxSpeed;
    public float JumpHeight;
    private bool isGrounded;
    private bool isOnWall;
    public float FallingSpeedOnWall;

    private Rigidbody rb;
    private const float groundDrag = 8f;
    private const float airDrag = 0.1f;
    private bool isCameraTilted = false;

    private const float FOVChange = 10;
    private float baseFOV;
    private CameraManager camManager;

    private BoxCollider feetCollider;
    private BoxCollider rightCollider;
    private BoxCollider leftCollider;
    private GameObject rightBox;
    private GameObject leftBox;

    private BoxCollider interactCollider;
    private Vector3 velocityHolder;
    private Vector3 vaultStart;
    private Vector3 vaultTarget;

    private Vector3 runIntoWallVelocity;		
    private Vector3 playerMovementOnWall;

    public enum MovementState
    {
        Idle,
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
	    Right,
	    None
	}

    private MovementState myMovementState = MovementState.Idle;
    private WallSide myWallSide;



    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        baseFOV = Camera.main.fieldOfView;
        feetCollider = GameObject.Find("FeetCollider").GetComponent<BoxCollider>();
        rightCollider = GameObject.Find("SideColliderRight").GetComponent<BoxCollider>();
        leftCollider = GameObject.Find("SideColliderLeft").GetComponent<BoxCollider>();
        interactCollider = GameObject.Find("InteractCollider").GetComponent<BoxCollider>();
        if (GetComponent<CameraManager>() != null)
        {
            camManager = GetComponent<CameraManager>();
        }
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
            HandleCamera();
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
                isOnWall = false;
                isGrounded = true;
                rb.useGravity = true;
                if (myMovementState == MovementState.WallRun)
                {
                    SwitchState(MovementState.Run);
                }
                if (isCameraTilted == true)
                {
                    camManager.ResetRotation();
                    isCameraTilted = false;
                }
                break;
            }
        }
    }

    private void HandleWall()
    {
        /*isOnWall = false;
        rb.useGravity = true;
        Collider[] allOverlappingCollidersLeft = Physics.OverlapBox(rightCollider.bounds.center, rightCollider.bounds.extents);

        foreach (Collider collidedObject in allOverlappingCollidersLeft)
        {
            if (collidedObject.tag == "Wall" && isGrounded == false)
            {
                Debug.Log("Character is on a wall left");
                isOnWall = true;
                rb.useGravity = false;
                if (myMovementState != MovementState.WallRun)
                {
                    SwitchState(MovementState.WallRun);
                }
                break;
            }
        }*/
    }

    private void CheckForWall()
    {
        isOnWall = false;
        RaycastHit hit;
        Debug.DrawRay(transform.position, new Vector3(rb.velocity.x, 0, rb.velocity.z), Color.red, 99);
        if (Physics.Raycast(transform.position, new Vector3(rb.velocity.x, 0, rb.velocity.z), out hit, 0.5f))
        {
            if (hit.transform.tag == "Wall")
            {
                if (Vector2.Angle(new Vector2(rb.velocity.x, rb.velocity.z), hit.normal) >= 130)
                {
                    Debug.DrawRay(rb.position, rb.position, Color.red, 20);
                    isOnWall = true;
                    runIntoWallVelocity = rb.velocity;
                    SwitchState(MovementState.WallRun);
                }
            }
        }


        Collider[] allOverlappingCollidersRight = Physics.OverlapBox(leftCollider.bounds.center, leftCollider.bounds.extents);

        foreach (Collider collidedObject in allOverlappingCollidersRight)
        {
            if (collidedObject.tag == "Wall" && isGrounded == false)
            {
                Debug.Log("Character is on a wall right");
                isOnWall = true;
                rb.useGravity = false;
                if (myMovementState != MovementState.WallRun)
                {
                    SwitchState(MovementState.WallRun);
                }
                break;
            }
        }
    }

    private void HandleInput()
    {
        if (myMovementState == MovementState.WallRun)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (myWallSide == WallSide.Right)
                { rb.AddForce(transform.right * JumpHeight / 2); rb.AddForce(transform.up * JumpHeight / 2); }
                if (myWallSide == WallSide.Left)
                { rb.AddForce(-transform.right * JumpHeight / 2); rb.AddForce(transform.up * JumpHeight / 2); }
                isOnWall = false;
                myMovementState = MovementState.Fall;
                Debug.Log("Wall is :" + myWallSide.ToString());
            }
        }
        /*if (isOnWall)
        {
            rb.useGravity = false;
            isGrounded = true;
        }
        else
        {
            rb.useGravity = true;
        }*/
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
                    if ((myMovementState == MovementState.Idle|| myMovementState == MovementState.Run) && isGrounded)
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
    }

    private void HandleCamera()
    {
        if (rb.velocity.magnitude >= 0.2f)
        {
            if (Vector2.Angle(new Vector2(rb.velocity.x, rb.velocity.z), new Vector2(transform.forward.x, transform.forward.z)) <= 70)
            {
                camManager.SetFOV(baseFOV + ((FOVChange * (rb.velocity.magnitude / MaxSpeed)) * (1 - (Vector2.Angle(new Vector2(rb.velocity.x, rb.velocity.z), new Vector2(transform.forward.x, transform.forward.z)) / 70))));
            }
        }
        else
        {
            camManager.SetFOV(baseFOV);
        }
    }

    private void SwitchState(MovementState state)
    {
        switch (state)
        {
            case MovementState.Idle:
                Debug.Log("idling");
                myMovementState = MovementState.Idle;
                break;
            case MovementState.Run:
                Debug.Log("running");
                myMovementState = MovementState.Run;
                break;
            case MovementState.Jump:
                Debug.Log("jumping");
                myMovementState = MovementState.Jump;
                break;
            case MovementState.Fall:
                Debug.Log("falling");
                myMovementState = MovementState.Fall;
                break;
            case MovementState.Vault:
                Debug.Log("vaulting");
                myMovementState = MovementState.Vault;
                break;
            case MovementState.Slide:
                Debug.Log("sliding");
                myMovementState = MovementState.Slide;
                break;
            case MovementState.WallRun:
                Debug.Log("wall running");
                myMovementState = MovementState.WallRun;
                break;
            case MovementState.WallCling:
                Debug.Log("wall clinging");
                myMovementState = MovementState.WallCling;
                break;
            default:
                break;
        }
    }

    private void HandleState()
    {
        //Debug.Log(new Vector2(rb.velocity.x, rb.velocity.z).magnitude);
        //Debug.Log(myMovementState);
        switch (myMovementState)
        {
            case MovementState.Idle:
                if (new Vector2(rb.velocity.x, rb.velocity.z).magnitude > 0.1f)
                {
                    SwitchState(MovementState.Run);
                }
                break;
            case MovementState.Run:
                if (new Vector2(rb.velocity.x, rb.velocity.z).magnitude <= 0.1f)
                {
                    SwitchState(MovementState.Idle);
                }
                camManager.CameraBob();
                //cameraBob();
                break;
            case MovementState.Jump:
                Debug.Log(rb.velocity.y);
                if (rb.velocity.y < 0)
                {
                    SwitchState(MovementState.Fall);
                }
                RaycastHit hit;
                Debug.DrawRay(transform.position, new Vector3(rb.velocity.x, 0 , rb.velocity.z), Color.red, 99);
                if (Physics.Raycast(transform.position, new Vector3(rb.velocity.x, 0, rb.velocity.z), out hit, 0.5f))
                {
                    if (hit.transform.tag == "Wall")
                    {
                        if (Vector2.Angle(new Vector2(rb.velocity.x, rb.velocity.z), hit.normal) >= 130)
                        {
                            
                        }
                    }
                }
                break;
            case MovementState.Fall:
                if (isGrounded)
                {
                    if (new Vector2(rb.velocity.x, rb.velocity.z).magnitude > 0)
                    {
                        SwitchState(MovementState.Run);
                    }
                    else
                    {
                        SwitchState(MovementState.Idle);
                    }
                }
                if (rb.velocity.y > 0)
                {
                    SwitchState(MovementState.Jump);
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
                        SwitchState(MovementState.Run);
                    }
                    else
                    {
                        SwitchState(MovementState.Idle);
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
                        SwitchState(MovementState.Run);
                    }
                    else
                    {
                        SwitchState(MovementState.Idle);
                    }
                }
                break;
            case MovementState.WallRun:

                rb.AddForce(new Vector3(runIntoWallVelocity.x, 0, runIntoWallVelocity.z) * MovementSpeedInAir * Time.deltaTime);
                rb.AddForce(transform.up * 3);

                /*rb.AddForce(transform.forward * MovementSpeed * Time.deltaTime);
                
                if (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude >= MaxSpeed)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * MaxSpeed + new Vector3(0, rb.velocity.y, 0);
                }
                rb.AddForce(-transform.up * FallingSpeedOnWall * Time.deltaTime);*/
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
        SwitchState(MovementState.Vault);
    }

    public void Slide(Vector3 start, Vector3 end)
    {
        velocityHolder = rb.velocity;
        rb.isKinematic = true;
        transform.position += new Vector3(0, -1.5f, 0);
        vaultStart = transform.position;
        vaultTarget = transform.position + ((end - start).normalized * Vector3.Distance(transform.position, end));
        SwitchState(MovementState.Slide);
    }

    public void Jump()
    {
        rb.AddForce(transform.up * JumpHeight, ForceMode.Impulse);
        SwitchState(MovementState.Jump);
    }

    public void WallCling()
	{
        SwitchState(MovementState.WallCling);
	    rb.velocity -= transform.up * FallingSpeedOnWall * Time.deltaTime;
	}

    public MovementState GetMovementState()
	{		
	    return myMovementState;		
	}
}
