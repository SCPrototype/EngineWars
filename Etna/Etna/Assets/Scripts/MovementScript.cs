using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    public float MovementSpeed;

    private Vector3 _movement = new Vector3();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * MovementSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * MovementSpeed;

        transform.Translate(x, 0, z);

        if(Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(0, 90, 0);
        }
    }
}
