using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCrystalHandler : MonoBehaviour
{

    public GameObject gameHandler;
    public GameObject handCrystal;
    public float LightIntensity;
    public float LightRadius;
    private Light handCrystalLight;
    private Resources resources;
    private bool isFlickering = false;
    private bool lightIsOn = false;
    float timer;

    // Use this for initialization
    void Start()
    {
        gameHandler = GameObject.Find("GameHandler");
        resources = gameHandler.GetComponent<Resources>();
        handCrystalLight = handCrystal.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleLight();
        if (resources.GetAmountOfEnergy() < 10 && resources.GetAmountOfEnergy() > 0 && isFlickering == false)
        {
            FlickerLight();
        }
        else
        {
            isFlickering = false;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "CollectibleLight")
        {
            resources.AddEnergy(20);
            lightIsOn = true;
            Destroy(col.gameObject);
        }
    }

    private void FlickerLight()
    {
        isFlickering = true;
        if (resources.GetAmountOfEnergy() < 0)
        {
            lightIsOn = false;
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                if (lightIsOn == true)
                {

                    lightIsOn = false;
                }
                else
                {

                    lightIsOn = true;
                }
                timer = 0;
            }
        }
    }

    private void HandleLight()
    {
        if (lightIsOn == true)
        {
            if (null != handCrystalLight)
            {
                handCrystalLight.intensity = 10;
            }
        }
        else
        {
            handCrystalLight.intensity = 0;
        }
    }
}
