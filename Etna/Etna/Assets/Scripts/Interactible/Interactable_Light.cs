using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Interactable_Light : Interactable
{
    private AudioSource myAudioSource;
    public ActivationLight activationLight;

    // Use this for initialization
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(PlayerMovement target)
    {
        Debug.Log("Static crystal clicked");
        activationLight.ActivateLight();
        if (activationLight.GetIsActivated())
        {
            myAudioSource.Play();
        }
    }
}
