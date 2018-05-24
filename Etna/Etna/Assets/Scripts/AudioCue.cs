using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(BoxCollider))]
public class AudioCue : MonoBehaviour {

    public static bool isPlaying = false;
    private bool iAmPlaying = false;
    public bool endTutorial;

    private AudioSource myAudioSource;
    public bool singleUse;
    public bool shouldWait;
    private bool used;

	// Use this for initialization
	void Start () {
        myAudioSource = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
        if (iAmPlaying)
        {
            if (!myAudioSource.isPlaying)
            {
                isPlaying = false;
                iAmPlaying = false;
                if (endTutorial)
                {
                    GameMenu_Handler.StartRealLevel();
                }
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (!isPlaying || !shouldWait)
        {
            if (other.tag == "Player")
            {
                if (singleUse)
                {
                    if (!used)
                    {
                        myAudioSource.Play();
                        if (shouldWait)
                        {
                            isPlaying = true;
                            iAmPlaying = true;
                        }
                        if (endTutorial)
                        {
                            iAmPlaying = true;
                            GameMenu_Handler UIHandler = GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameMenu_Handler>();
                            UIHandler.FadeToBlack(true, 3);
                        }
                        used = true;
                    }
                }
                else
                {
                    myAudioSource.Play();
                    if (shouldWait)
                    {
                        isPlaying = true;
                        iAmPlaying = true;
                    }
                    if (endTutorial)
                    {
                        iAmPlaying = true;
                        GameMenu_Handler UIHandler = GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameMenu_Handler>();
                        UIHandler.FadeToBlack(true, 3);
                    }
                }
            }
        }
    }
}
