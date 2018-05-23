using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationLight : MonoBehaviour
{

    public Light Torchlight;
    private bool _isActivated;
    public float maxLightIntensity;
    public float maxLightRange;
    public float TimeTillExtinguish;
    private float timeLeft = 0;

    private GameObject gameHandler;
    private Resources resources;

    // Use this for initialization
    void Start()
    {
        _isActivated = false;
        gameHandler = GameObject.Find("GameHandler");
        resources = gameHandler.GetComponent<Resources>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActivated)
        {
            timeLeft -= Time.deltaTime;

            Torchlight.intensity = maxLightIntensity * (timeLeft / TimeTillExtinguish);
            if (timeLeft <= 0)
            {
                TurnOffLight();
            }
        }
    }

    public void ActivateLight()
    {
        if (resources.CanActivateLight() == true)
        {
            _isActivated = true;
            Torchlight.intensity = maxLightIntensity;
            timeLeft = TimeTillExtinguish;
        }

    }

    public void TurnOffLight()
    {
        _isActivated = false;
        Torchlight.intensity = 0;
    }

    public bool GetIsActivated()
    {
        return _isActivated;
    }
}
