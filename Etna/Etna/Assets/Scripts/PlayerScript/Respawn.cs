using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

[RequireComponent(typeof(PlayerMovement))]
public class Respawn : MonoBehaviour {

    private PlayerMovement myPlayer;
    private Vector3 respawnPos;
    private BoxCollider darknessCollider;
    private VoiceLinePlayer voiceLinePlayer;

	// Use this for initialization
	void Start () {
        respawnPos = transform.position;
        myPlayer = GetComponent<PlayerMovement>();
        voiceLinePlayer = GameObject.Find("VoiceLinePlayer").GetComponent<VoiceLinePlayer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameManager.GameOver) {
            if (Input.GetKeyDown(KeyCode.R))
            {
                DoRespawn();
            }
        }
	}

    public void DoRespawn()
    {
        if (darknessCollider != null)
        {
            if (!darknessCollider.bounds.Contains(respawnPos))
            {
                myPlayer.GetRigidBody().velocity = new Vector3(0,0,0);
                transform.position = respawnPos;
            }
            else if (!GameManager.Consuming)
            {
                GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                gm.QuickDarknessConsume();
                voiceLinePlayer.PlayVoiceLine(2);
            }
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
