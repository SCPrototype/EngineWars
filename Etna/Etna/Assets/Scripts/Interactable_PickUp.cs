using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_PickUp : Interactable
{
    public GameObject pickUp;
    private GameObject gameHandler;
    private Resources resources;

    // Use this for initialization
    void Start()
    {
        gameHandler = GameObject.Find("GameHandler");
        resources = gameHandler.GetComponent<Resources>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(PlayerMovement target)
    {
        Debug.Log("Pickup light.");
        resources.AddEnergy(20);
        Destroy(pickUp);
    }
}
