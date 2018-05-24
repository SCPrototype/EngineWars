using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHandler : MonoBehaviour
{

    private GameObject gameHandler;
    private Resources resources;
    private Light crystalLight;
    public float maxLightIntensity;
    public float maxLightRange;

    // Use this for initialization
    void Start()
    {
        crystalLight = GetComponent<Light>();
        gameHandler = GameObject.Find("GameHandler");
        resources = gameHandler.GetComponent<Resources>();
    }

    // Update is called once per frame
    void Update()
    {
        if (null == gameHandler)
        {
            gameHandler = GameObject.Find("GameHandler");
        }
        else if (null == resources)
        {
            resources = gameHandler.GetComponent<Resources>();
        }
        else if (null != crystalLight)
        {
            crystalLight.intensity = maxLightIntensity * (resources.GetAmountOfEnergy() / 100);
        }
    }
}
