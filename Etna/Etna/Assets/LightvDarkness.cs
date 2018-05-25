using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightvDarkness : MonoBehaviour {

    public Image lightvdarkness;
    public Resources resources;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float light = resources.GetAmountOfEnergy();
        lightvdarkness.fillAmount = light / 100;
	}
}
