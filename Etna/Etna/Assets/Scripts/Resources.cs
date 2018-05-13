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
    private Text lightUItext;
    private GameObject UI;

    // Use this for initialization
    void Start()
    {
        UI = GameObject.Find("UI Canvas");
        lightUItext = UI.GetComponent<Text>();
        if (null == UI)
        {
            Debug.Log("UI not found");
        }
        if (null == lightUItext)
        {
            Debug.Log("Light text not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lightUItext != null)
        {
            lightUItext.text = amountOfEnergyStored.ToString();
            timeLeft += Time.deltaTime;
            if (timeLeft >= amountOfTimeLightDecay && amountOfEnergyStored > 0)
            {
                amountOfEnergyStored--;
                timeLeft = 0;
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
        if (amountOfEnergyStored >= 20)
        {
            amountOfEnergyStored -= 20;
            return true;
        }
        else
        {
            return false;
        }
    }
}
