using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject DarknessPrefab;
    private float darknessSpeed;
    public float BaseDarknessSpeed;
    public float DarknessSpeedIncrease;
    public float DarknessSpeedDecrease;
    private List<Vector3> darknessPath = new List<Vector3>();
    private GameObject darkness;

	// Use this for initialization
	void Start () {
        darkness = GameObject.Instantiate(DarknessPrefab, transform);
        darknessSpeed = BaseDarknessSpeed;

    }
	
	// Update is called once per frame
	void Update () {
        if (darknessPath.Count > 0)
        {
            moveDarknessAlongPath();
        }
        darknessSpeed += DarknessSpeedIncrease;
    }

    private void moveDarknessAlongPath()
    {
        if ((darkness.transform.position - darknessPath[0]).magnitude <= darknessSpeed)
        {
            darkness.transform.position = darknessPath[0];
            darknessPath.RemoveAt(0);
        }
        else
        {
            darkness.transform.position += (darknessPath[0] - darkness.transform.position).normalized * darknessSpeed;
        }
    }

    public void AddToDarknessPath(Vector3 target)
    {
        darknessPath.Add(target + new Vector3(0, 25, 0));
    }

    public void SlowDownDarkness()
    {
        darknessSpeed -= DarknessSpeedDecrease;
        if (darknessSpeed < BaseDarknessSpeed)
        {
            darknessSpeed = BaseDarknessSpeed;
        }
    }
}
