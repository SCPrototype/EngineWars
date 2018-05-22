using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassMovement : MonoBehaviour
{

    public GameObject bar;
    private RectTransform compassBar;
    private GameObject player;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        if (null == player)
        {
            Debug.Log("Player not found in CompassMovement");
        }
        compassBar = bar.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float widthOfBar = (Screen.width - compassBar.rect.width * bar.transform.localScale.x) / 2;
        float centerX = Mathf.Lerp(widthOfBar, Screen.width - widthOfBar, (float)(player.transform.eulerAngles.y / 360 + 0.5) % 1);
        this.transform.position = new Vector3(centerX, bar.transform.position.y, 0);
    }
}
