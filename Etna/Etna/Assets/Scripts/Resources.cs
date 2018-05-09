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
    public Text lightUI;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lightUI.text = amountOfEnergyStored.ToString();
        timeLeft += Time.deltaTime;
        if (timeLeft >= amountOfTimeLightDecay && amountOfEnergyStored > 0)
        {
            amountOfEnergyStored--;
            timeLeft = 0;
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
        if(amountOfEnergyStored >= 20)
        { amountOfEnergyStored -= 20;
            return true;
        } else
        {
            return false;
        }
    }
}
