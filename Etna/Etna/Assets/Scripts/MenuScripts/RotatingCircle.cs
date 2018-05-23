using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCircle : MonoBehaviour {

    public float speed;
    public Rigidbody2D rb2D;

    // Use this for initialization
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2D.MoveRotation(rb2D.rotation + speed * Time.fixedDeltaTime);
    }
}
