using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Light : Interactable
{
    public ActivationLight activationLight;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(PlayerMovement target)
    {
        Debug.Log("Static crystal clicked");
        activationLight.ActivateLight();
        
    }
}
