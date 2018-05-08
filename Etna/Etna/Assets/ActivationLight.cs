using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationLight : MonoBehaviour
{

    public Light light;
    private bool _isActivated;
    public float maxLightIntensity;
    public float maxLightRange;
    public float TimeTillExtinguish;
    private float timeLeft = 0;

    // Use this for initialization
    void Start()
    {
        _isActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActivated)
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft <= 0)
            {
                TurnOffLight();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ActivateLight();
        }
    }

    public void ActivateLight()
    {
        _isActivated = true;
        light.intensity = maxLightIntensity;
        timeLeft = TimeTillExtinguish;
    }

    public void TurnOffLight()
    {
        _isActivated = false;
        light.intensity = 0;
    }
}
