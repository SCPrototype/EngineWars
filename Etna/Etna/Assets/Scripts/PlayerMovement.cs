using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    public float MovementSpeed;
    public float MovementSpeedInAir;
    public float MaxSpeed;
    public float JumpHeight;
    private bool isGrounded;
    private bool isOnWall;
    private GameObject currentWall;
    private GameObject prevWall;
    public float FallingSpeedOnWall;

    private Rigidbody rb;
    private const float groundDrag = 8f;
    private const float airDrag = 0.1f;

    private const float FOVChange = 10;
    private float baseFOV;
    private CameraManager camManager;

    private SphereCollider rangeCollider;

    private BoxCollider interactCollider;
    private Vector3 velocityHolder;
    private Vector3 vaultStart;
    private Vector3 vaultTarget;

    private Vector3 runIntoWallVelocity;		
    private Vector3 playerMovementOnWall;
    public float JumpFromWallStrength;
    private bool isCameraTilted = false;

    public enum MovementState
    {
        Idle,
        Run,
        Jump,
        Fall,
        Vault,
        Slide,
        WallRun,
        WallClimb,
        WallJump
    }

    private enum WallSide
	{
	    Left,
	    Right,
        Front,
	    None
	}

    private MovementState myMovementState = MovementState.Idle;
    private WallSide myWallSide;

    private AudioSource myAudioSource;
    public AudioClip JumpSound;
    public AudioClip LandingSound;
    public AudioClip SlideSound;
    public AudioClip RunSound;
    private const float pitchShift = 0.05f;
    private const float idleThreshold = 0.1f;
    private const float groundedCheckDist = 0.2f;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
        baseFOV = Camera.main.fieldOfView;
        rangeCollider = GameObject.Find("RangeCollider").GetComponent<SphereCollider>();
        interactCollider = GameObject.Find("InteractCollider").GetComponent<BoxCollider>();
        if (GetComponent<CameraManager>() != null)
        {
            camManager = GetComponent<CameraManager>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameMenu_Handler.Paused)
        {
            if (rb.isKinematic && myMovementState != MovementState.Vault && myMovementState != MovementState.Slide)
            {
                rb.isKinematic = false;
            }
            HandleInput();
            HandleGrounded();
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
        if (myMovementState != MovementState.Jump)
        {
            RaycastHit[] hit = new RaycastHit[5];
            Debug.DrawRay(transform.position, new Vector3(0, -transform.lossyScale.y - groundedCheckDist, 0), Color.red, 5);
            Debug.DrawRay(transform.position + (transform.forward + transform.right) * 0.4f, new Vector3(0, -transform.lossyScale.y - groundedCheckDist, 0), Color.red, 5);
            Debug.DrawRay(transform.position + (-transform.forward + transform.right) * 0.4f, new Vector3(0, -transform.lossyScale.y - groundedCheckDist, 0), Color.red, 5);
            Debug.DrawRay(transform.position + (-transform.forward - transform.right) * 0.4f, new Vector3(0, -transform.lossyScale.y - groundedCheckDist, 0), Color.red, 5);
            Debug.DrawRay(transform.position + (transform.forward - transform.right) * 0.4f, new Vector3(0, -transform.lossyScale.y - groundedCheckDist, 0), Color.red, 5);
            if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit[0], transform.lossyScale.y + groundedCheckDist) ||
                Physics.Raycast(transform.position + (transform.forward + transform.right) * 0.4f, new Vector3(0, -1, 0), out hit[1], transform.lossyScale.y + groundedCheckDist) ||
                Physics.Raycast(transform.position + (-transform.forward + transform.right) * 0.4f, new Vector3(0, -1, 0), out hit[2], transform.lossyScale.y + groundedCheckDist) ||
                Physics.Raycast(transform.position + (-transform.forward - transform.right) * 0.4f, new Vector3(0, -1, 0), out hit[3], transform.lossyScale.y + groundedCheckDist) ||
                Physics.Raycast(transform.position + (transform.forward - transform.right) * 0.4f, new Vector3(0, -1, 0), out hit[4], transform.lossyScale.y + groundedCheckDist))
            {
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].transform != null)
                    {
                        if (hit[i].transform.tag == "Ground")
                        {
                            if (!isGrounded)
                            {
                                isGrounded = true;
                                myAudioSource.clip = LandingSound;
                                myAudioSource.loop = false;
                                myAudioSource.pitch = UnityEngine.Random.Range(1 - pitchShift, 1 + pitchShift);
                                myAudioSource.Play();
                                prevWall = null;
                            }
                            isOnWall = false;
                            rb.useGravity = true;
                            if (myMovementState == MovementState.WallRun || myMovementState == MovementState.WallClimb)
                            {
                                SwitchState(MovementState.Run);
                            }
                            if (isCameraTilted == true)
                            {
                                camManager.ResetRotation(rb);
                                isCameraTilted = false;
                            }
                            return;
                        }
                    }
                }
            }
        }
        isGrounded = false;
    }

    private void CheckForWall()
    {
        isOnWall = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(rb.velocity.x, 0, rb.velocity.z), out hit, 4))
        {
            if (hit.transform.tag == "Wall" && hit.transform.gameObject != prevWall)
            {
                Collider[] collidedObjects = Physics.OverlapSphere(rangeCollider.transform.position, rangeCollider.radius);
                for (int i = 0; i < collidedObjects.Length; i++)
                {
                    if (collidedObjects[i] == hit.collider)
                    {
                        if (Vector2.Angle(new Vector2(rb.velocity.x, rb.velocity.z), new Vector2(hit.normal.x, hit.normal.z)) <= 150 && Vector2.Angle(new Vector2(rb.velocity.x, rb.velocity.z), new Vector2(hit.normal.x, hit.normal.z)) > 90)
                        {
                            isOnWall = true;
                            currentWall = hit.transform.gameObject;
                            runIntoWallVelocity = rb.velocity;
                            SwitchState(MovementState.WallRun);

                            if (Vector2.Angle(new Vector2(transform.right.x, transform.right.z), new Vector2(hit.normal.x, hit.normal.z)) >= 90)
                            {
                                myWallSide = WallSide.Right;
                                camManager.Rotate(20, rb, -hit.normal);
                            }
                            else
                            {
                                myWallSide = WallSide.Left;
                                camManager.Rotate(-20, rb, hit.normal);
                            }
                            isCameraTilted = true;
                        }
                        else if (Vector2.Angle(new Vector2(rb.velocity.x, rb.velocity.z), new Vector2(hit.normal.x, hit.normal.z)) <= 180)
                        {
                            isOnWall = true;
                            currentWall = hit.transform.gameObject;
                            runIntoWallVelocity = rb.velocity;
                            myWallSide = WallSide.Front;
                            SwitchState(MovementState.WallClimb);
                            //Wall climb
                        }
                        break;
                    }
                }
            }
        }
    }

    private void HandleInput()
    {
        if (myMovementState == MovementState.WallRun || myMovementState == MovementState.WallClimb)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                if (myWallSide == WallSide.Right)
                {
                    //rb.AddForce(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * JumpFromWallStrength * Time.fixedDeltaTime);
                    if (Input.GetKey(KeyCode.A))
                    {
                        rb.AddForce(-transform.right * JumpFromWallStrength * Time.fixedDeltaTime);
                    }
                    rb.AddForce(transform.up * JumpFromWallStrength * Time.fixedDeltaTime);
                    camManager.ResetRotation(rb);
                }
                else if (myWallSide == WallSide.Left)
                {
                    //rb.AddForce(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * JumpFromWallStrength * Time.fixedDeltaTime);
                    if (Input.GetKey(KeyCode.D))
                    {
                        rb.AddForce(transform.right * JumpFromWallStrength * Time.fixedDeltaTime);
                    }
                    rb.AddForce(transform.up * JumpFromWallStrength * Time.fixedDeltaTime);
                    camManager.ResetRotation(rb);
                }
                else if (myWallSide == WallSide.Front)
                {
                    RaycastHit hit;
                    Debug.DrawRay(new Vector3(transform.position.x, currentWall.transform.position.y, transform.position.z), new Vector3(currentWall.transform.position.x, transform.position.y, currentWall.transform.position.z) - transform.position, Color.green, 15);
                    if (Physics.Raycast(new Vector3(transform.position.x, currentWall.transform.position.y, transform.position.z), new Vector3(currentWall.transform.position.x, transform.position.y, currentWall.transform.position.z) - transform.position, out hit, 4))
                    {
                        if (hit.transform.gameObject == currentWall)
                        {
                            rb.AddForce(hit.normal * JumpFromWallStrength * Time.fixedDeltaTime);
                        } else
                        {
                            rb.AddForce(-transform.forward * JumpFromWallStrength * Time.fixedDeltaTime);
                        }
                    } else
                    {
                        rb.AddForce(-transform.forward * JumpFromWallStrength * Time.fixedDeltaTime);
                    }
                    // rb.AddForce(transform.right * JumpFromWallStrength / 2);
                    rb.AddForce(transform.up * JumpFromWallStrength * 1.5f * Time.fixedDeltaTime);
                    camManager.ResetRotation(rb);
                    //SWITCH: Turn camera around on wall jump
                    //camManager.TurnAround();
                }
                prevWall = currentWall;
                currentWall = null;
                isOnWall = false;
                SwitchState(MovementState.WallJump);
                myWallSide = WallSide.None;
            }
        }
        else if (isGrounded)
        {
            if (myMovementState != MovementState.Vault && myMovementState != MovementState.Slide)
            {
                rb.drag = groundDrag;
                if (Input.GetKey(KeyCode.W))
                {
                    //rb.velocity += transform.forward * MovementSpeed * Time.deltaTime;
                    rb.AddForce(transform.forward * MovementSpeed * Time.fixedDeltaTime);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    rb.AddForce(-transform.right * MovementSpeed * Time.fixedDeltaTime);
                    //rb.velocity += -transform.right * MovementSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    rb.AddForce(-transform.forward * MovementSpeed * Time.fixedDeltaTime);
                    //rb.velocity += -transform.forward * MovementSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    rb.AddForce(transform.right * MovementSpeed * Time.fixedDeltaTime);
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
                }
            }
        }
        else
        {
            rb.drag = airDrag;
            if (Input.GetKey(KeyCode.W))
            {
                //rb.velocity += transform.forward * MovementSpeed * Time.deltaTime * 0.003f;
                rb.AddForce(transform.forward * MovementSpeedInAir * Time.fixedDeltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(-transform.right * MovementSpeedInAir * Time.fixedDeltaTime);
                //rb.velocity += -transform.right * MovementSpeed * Time.deltaTime * 0.003f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(-transform.forward * MovementSpeedInAir * Time.fixedDeltaTime);
                //rb.velocity += -transform.forward * MovementSpeed * Time.deltaTime * 0.003f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(transform.right * MovementSpeedInAir * Time.fixedDeltaTime);
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
        //Debug.Log("Switching state from :" + myMovementState + ": to :" + state + ":");
        switch (state)
        {
            case MovementState.Idle:
                myMovementState = MovementState.Idle;
                break;
            case MovementState.Run:
                myMovementState = MovementState.Run;
                break;
            case MovementState.Jump:
                myMovementState = MovementState.Jump;
                break;
            case MovementState.Fall:
                myMovementState = MovementState.Fall;
                break;
            case MovementState.Vault:
                myMovementState = MovementState.Vault;
                break;
            case MovementState.Slide:
                myMovementState = MovementState.Slide;
                break;
            case MovementState.WallRun:
                myMovementState = MovementState.WallRun;
                break;
            case MovementState.WallClimb:
                if (myMovementState != MovementState.WallClimb && myMovementState != MovementState.Fall && myMovementState != MovementState.WallJump)
                {
                    rb.velocity = new Vector3(0, runIntoWallVelocity.magnitude, 0);
                }
                myMovementState = MovementState.WallClimb;
                break;
            case MovementState.WallJump:
                myAudioSource.clip = JumpSound;
                myAudioSource.loop = false;
                myAudioSource.pitch = UnityEngine.Random.Range(1 - pitchShift, 1 + pitchShift);
                myAudioSource.Play();
                myMovementState = MovementState.WallJump;
                break;
            default:
                break;
        }
    }

    private void HandleState()
    {
        switch (myMovementState)
        {
            case MovementState.Idle:
                if (new Vector2(rb.velocity.x, rb.velocity.z).magnitude > idleThreshold)
                {
                    SwitchState(MovementState.Run);
                }
                if (!isGrounded)
                {
                    SwitchState(MovementState.Fall);
                }
                break;
            case MovementState.Run:
                camManager.CameraBob();
                if (!myAudioSource.isPlaying)
                {
                    myAudioSource.clip = RunSound;
                    myAudioSource.loop = true;
                    myAudioSource.pitch = 1;
                    myAudioSource.Play();
                }
                if (new Vector2(rb.velocity.x, rb.velocity.z).magnitude <= idleThreshold)
                {
                    SwitchState(MovementState.Idle);
                }
                if (!isGrounded)
                {
                    SwitchState(MovementState.Fall);
                }
                break;
            case MovementState.Jump:
                CheckForWall();
                if (rb.velocity.y < 0)
                {
                    SwitchState(MovementState.Fall);
                }
                if (isGrounded)
                {
                    if (new Vector2(rb.velocity.x, rb.velocity.z).magnitude > idleThreshold)
                    {
                        SwitchState(MovementState.Run);
                    }
                    else
                    {
                        SwitchState(MovementState.Idle);
                    }
                }
                break;
            case MovementState.Fall:
                if (rb.velocity.y >= -0.5f)
                {
                    CheckForWall();
                }
                if (isGrounded)
                {
                    if (new Vector2(rb.velocity.x, rb.velocity.z).magnitude > idleThreshold)
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
                transform.position += (vaultTarget - vaultStart) * Time.fixedDeltaTime * (0.5f + (velocityHolder.magnitude / 4));
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
                transform.position += (vaultTarget - vaultStart) * Time.fixedDeltaTime * (0.5f + (velocityHolder.magnitude / 4));
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
                Collider[] collidedObjects = Physics.OverlapSphere(rangeCollider.transform.position, rangeCollider.radius);
                for (int i = 0; i < collidedObjects.Length; i++)
                {
                    if (collidedObjects[i].gameObject == currentWall)
                    {
                        break;
                    }
                    if (i == collidedObjects.Length -1)
                    {
                        camManager.ResetRotation(rb);
                        SwitchState(MovementState.Jump);
                    }
                }
                rb.AddForce(transform.forward * new Vector3(runIntoWallVelocity.x, 0, runIntoWallVelocity.z).magnitude * Time.fixedDeltaTime);
                //rb.AddForce(new Vector3(runIntoWallVelocity.x, 0, runIntoWallVelocity.z) * MovementSpeedInAir * Time.fixedDeltaTime);
                rb.AddForce(transform.up * 4 * Time.fixedDeltaTime);

                /*rb.AddForce(transform.forward * MovementSpeed * Time.deltaTime);
 
                if (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude >= MaxSpeed)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * MaxSpeed + new Vector3(0, rb.velocity.y, 0);
                }
                rb.AddForce(-transform.up * FallingSpeedOnWall * Time.deltaTime);*/
                break;
            case MovementState.WallClimb:
                collidedObjects = Physics.OverlapSphere(rangeCollider.transform.position, rangeCollider.radius);
                for (int i = 0; i < collidedObjects.Length; i++)
                {
                    if (collidedObjects[i].gameObject == currentWall)
                    {
                        break;
                    }
                    if (i == collidedObjects.Length - 1)
                    {
                        camManager.ResetRotation(rb);
                        SwitchState(MovementState.Jump);
                    }
                }
                break;
            case MovementState.WallJump:
                CheckForWall();
                if (rb.velocity.y < 0)
                {
                    SwitchState(MovementState.Fall);
                }
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
        myAudioSource.clip = SlideSound;
        myAudioSource.loop = false;
        myAudioSource.pitch = UnityEngine.Random.Range(1 - pitchShift, 1 + pitchShift);
        myAudioSource.Play();
        SwitchState(MovementState.Slide);
    }

    private void Jump()
    {
        rb.AddForce(transform.up * JumpHeight * Time.fixedDeltaTime, ForceMode.Impulse);
        /*myAudioSource.clip = JumpSound;
        myAudioSource.loop = false;
        myAudioSource.pitch = UnityEngine.Random.Range(1 - pitchShift, 1 + pitchShift);
        myAudioSource.Play();*/
        isGrounded = false;
        SwitchState(MovementState.Jump);
    }

    public MovementState GetMovementState()
	{		
	    return myMovementState;		
	}
}
