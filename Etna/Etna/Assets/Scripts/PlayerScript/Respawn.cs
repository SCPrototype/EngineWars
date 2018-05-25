using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMovement))]
public class Respawn : MonoBehaviour {

    private PlayerMovement myPlayer;
    private Vector3 respawnPos;
    private bool shouldRespawn = false;
    private BoxCollider darknessCollider;
    private VoiceLinePlayer voiceLinePlayer;

	// Use this for initialization
	void Start () {
        respawnPos = transform.position;
        myPlayer = GetComponent<PlayerMovement>();
        if (GameObject.Find("VoiceLinePlayer") != null)
        {
            voiceLinePlayer = GameObject.Find("VoiceLinePlayer").GetComponent<VoiceLinePlayer>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameManager.GameOver && !GameMenu_Handler.isFading) {
            if (Input.GetKeyDown(KeyCode.R))
            {
                DoRespawn();
            }
        }
        if (shouldRespawn)
        {
            if (!GameMenu_Handler.isFading) {
                shouldRespawn = false;
                myPlayer.GetRigidBody().velocity = new Vector3(0, 0, 0);
                transform.position = respawnPos;
                GameMenu_Handler UIHandler = GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameMenu_Handler>();
                UIHandler.FadeToBlack(false, 0.5f);
            }
        }
	}

    public void DoRespawn()
    {
        if (darknessCollider != null)
        {
            if (!darknessCollider.bounds.Contains(respawnPos))
            {
                GameMenu_Handler UIHandler = GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameMenu_Handler>();
                UIHandler.FadeToBlack(true, 0.5f);
                shouldRespawn = true;
                //transform.position = respawnPos;
            }
            else if (!GameManager.Consuming)
            {
                GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                gm.QuickDarknessConsume();
                if (voiceLinePlayer != null)
                {
                    voiceLinePlayer.PlayVoiceLine(2);
                }
            }
        } else
        {
            GameMenu_Handler UIHandler = GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameMenu_Handler>();
            UIHandler.FadeToBlack(true, 0.5f);
            shouldRespawn = true;
           // myPlayer.GetRigidBody().velocity = new Vector3(0, 0, 0);
            //transform.position = respawnPos;
        }
    }

    public void SetRespawnPos(Vector3 newPos)
    {
        respawnPos = newPos;
    }

    public void SetDarknessCollider(BoxCollider coll)
    {
        darknessCollider = coll;
    }
}
