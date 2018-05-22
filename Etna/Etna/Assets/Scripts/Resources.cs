using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{
    private float amountOfEnergyStored = 0;
    public float amountOfTimeLightDecay;
    private float timeLeft = 0;
    public float amountOfLightNeededToActivate;

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        DecreaseLight();
    }

    private void DecreaseLight()
    {
        if(amountOfEnergyStored > 0)
        {
            timeLeft += Time.deltaTime;
            if(timeLeft > amountOfTimeLightDecay)
            {
                timeLeft = 0;
                amountOfEnergyStored--;
            }
        }
    }

    public void AddEnergy(float amount)
    {
        amountOfEnergyStored += amount;
        if (amountOfEnergyStored > 100)
        {
            amountOfEnergyStored = 100;
        }
    }

    public float GetAmountOfEnergy()
    {
        return amountOfEnergyStored;
    }

    public bool CanActivateLight()
    {
        if (amountOfEnergyStored >= amountOfLightNeededToActivate)
        {
            amountOfEnergyStored -= amountOfLightNeededToActivate;
            return true;
        }
        else
        {
            return false;
        }
    }
}
