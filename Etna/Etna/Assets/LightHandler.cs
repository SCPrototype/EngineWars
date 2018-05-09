using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHandler : MonoBehaviour {

    public GameObject gameHandler;
    private Resources resources;
    private Light crystalLight;
    public float maxLightIntensity;
    public float maxLightRange;

    // Use this for initialization
    void Start () {
        crystalLight = GetComponent<Light>();
        resources = gameHandler.GetComponent<Resources>();
	}
	
	// Update is called once per frame
	void Update () {
        crystalLight.intensity = maxLightIntensity * (resources.GetAmountOfEnergy() / 100);
    }
}
