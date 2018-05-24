using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VoiceLinePlayer : MonoBehaviour {

    private AudioSource myAudioSource;
    public AudioClip[] voiceLines;

	// Use this for initialization
	void Start () {
        myAudioSource = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayVoiceLine(int indx)
    {
        if (myAudioSource == null)
        {
            myAudioSource = GetComponent<AudioSource>();
        }
        myAudioSource.clip = voiceLines[indx];
        myAudioSource.Play();
    }

    public bool GetIsPlaying()
    {
        return myAudioSource.isPlaying;
    }
}
