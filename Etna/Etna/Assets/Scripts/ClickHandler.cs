using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    private GameObject torchLight;
    private ActivationLight activationLight;

    // Use this for initialization
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 6.0f))
            {
                Debug.Log("Static crystal clicked");
                torchLight = hit.transform.gameObject;
                activationLight = torchLight.GetComponent<ActivationLight>();
                activationLight.ActivateLight();

                torchLight = null;
                activationLight = null;
            }
        }
    }
}
