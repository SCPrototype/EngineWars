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
        Debug.Log(resources);
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
            Debug.Log(crystalLight);
            Debug.Log(resources);
            crystalLight.intensity = maxLightIntensity * (resources.GetAmountOfEnergy() / 100);
        }
    }
}
