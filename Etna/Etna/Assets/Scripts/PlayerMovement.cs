using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(ParticleSystem))]
public class PlayerMovement : MonoBehaviour {

    public float MovementSpeed;
    public float MaxSpeed;
    private Rigidbody rb;

    private const float FOVChange = 25;
    private float baseFOV;
    private ParticleSystem dynamicLines;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        dynamicLines = GetComponent<ParticleSystem>();
        baseFOV = Camera.main.fieldOfView;

    }
	
	// Update is called once per frame
	void Update () {
        HandleInput();
        
    }

    private void HandleInput()
    {
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
        if (rb.velocity.magnitude >= 0.3f)
        {
            Camera.main.fieldOfView = baseFOV + (FOVChange * (rb.velocity.magnitude / MaxSpeed));
            dynamicLines.Play();
        }
        else
        {
            dynamicLines.Stop();
            Camera.main.fieldOfView = baseFOV;
        }
    }
}
