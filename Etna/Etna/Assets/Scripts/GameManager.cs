using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static bool GameOver = false;
    public static bool Consuming = false;

    public GameObject DarknessPrefab;
    private float darknessSpeed;
    public float BaseDarknessSpeed;
    public float DarknessSpeedIncrease;
    public float DarknessSpeedDecrease;
    private List<Vector3> darknessPath = new List<Vector3>();
    private GameObject darkness;
    private float timeSinceBlackOut;
    public const float BlackOutTime = 6f;

    // Use this for initialization
    void Start () {
        AudioListener.volume = 1;
        GameOver = false;
        Consuming = false;
        darkness = GameObject.Instantiate(DarknessPrefab, transform);
        darkness.SetActive(false);
        darkness.GetComponent<Darkness>().SetGameManager(this);
        darknessSpeed = BaseDarknessSpeed;
        DarknessEffects cameraDarknessEffects = Camera.main.GetComponent<DarknessEffects>();
        cameraDarknessEffects.SetTarget(darkness);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Respawn>().SetDarknessCollider(darkness.GetComponent<BoxCollider>());

        GameObject.Find("VoiceLinePlayer").GetComponent<VoiceLinePlayer>().PlayVoiceLine(0);
    }
	
	// Update is called once per frame
	void Update () {
        if (GameOver)
        {
            AudioListener.volume = 1 - ((Time.time - timeSinceBlackOut) / BlackOutTime);
        }
        else if (!GameMenu_Handler.Paused)
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
        if (!Consuming) {
            Debug.Log("DARKNESS IS SLOWED DOWN BY A LIGHT!");
            darknessSpeed -= DarknessSpeedDecrease;
            if (darknessSpeed < BaseDarknessSpeed)
            {
                darknessSpeed = BaseDarknessSpeed;
            }
        }
    }

    public void SetGameOver()
    {
        GameOver = true;
        timeSinceBlackOut = Time.time;
        //SceneManager.LoadScene("GameOverMenu");
    }

    public void QuickDarknessConsume()
    {
        DarknessSpeedIncrease = 5f;
        Consuming = true;
    }
}
