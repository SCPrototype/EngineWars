using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressAnyButton : MonoBehaviour {

    public GameObject NormalButton;
    public GameObject SelectedButton;

    private void Start()
    {
        //barspace.SetActive(false);
        SelectedButton.GetComponent<Image>().enabled = false;
        Debug.Log(SelectedButton.name);
    }

    private void Update()
    {

        if (Input.anyKey)
        {
            NormalButton.GetComponent<Image>().enabled = false;
            SelectedButton.GetComponent<Image>().enabled = true;
        }

        //to menunoselection

    }
}
