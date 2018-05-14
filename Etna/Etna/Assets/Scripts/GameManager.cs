using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        darkness.SetActive(false);
        darkness.GetComponent<Darkness>().SetGameManager(this);
        darknessSpeed = BaseDarknessSpeed;

    }
	
	// Update is called once per frame
	void Update () {
        if (!GameMenu_Handler.Paused)
        {
            if (darknessPath.Count > 0)
            {
                moveDarknessAlongPath();
            }
            darknessSpeed += DarknessSpeedIncrease * Time.deltaTime;
        }
    }

    private void moveDarknessAlongPath()
    {
        if ((darkness.transform.position - darknessPath[0]).magnitude <= darknessSpeed * Time.deltaTime)
        {
            darkness.transform.position = darknessPath[0];
            darknessPath.RemoveAt(0);
        }
        else
        {
            darkness.transform.position += (darknessPath[0] - darkness.transform.position).normalized * (darknessSpeed * Time.deltaTime);
        }
    }

    public void AddToDarknessPath(Vector3 target)
    {
        darknessPath.Add(target + new Vector3(0, 25, 0));
        if (!darkness.activeSelf && darknessPath.Count > 1)
        {
            darkness.transform.position = darknessPath[0] + (2 * (darknessPath[0] - darknessPath[1]));
            darkness.SetActive(true);
        }
    }

    public void SlowDownDarkness()
    {
        darknessSpeed -= DarknessSpeedDecrease;
        if (darknessSpeed < BaseDarknessSpeed)
        {
            darknessSpeed = BaseDarknessSpeed;
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOverMenu");
    }
}
