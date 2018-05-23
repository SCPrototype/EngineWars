using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.PostProcessing.Utilities;

[RequireComponent(typeof(PostProcessingBehaviour)), RequireComponent(typeof(PostProcessingController)), RequireComponent(typeof(AudioSource))]
public class DarknessEffects : MonoBehaviour {

    private const int maxVignetteDistance = 75;
    private const int minVignetteDistance = 30;
    private const float maxVignetteValue = 0.4f;
    private const float minVignetteValue = 0.65f;
    private const int maxCameraShakeDistance = 40;
    private const int minCameraShakeDistance = 30;
    private const float maxCameraShake = 0.05f;
    private const int warningDistance = 60;

    private bool shouldWarn = true;
    private const int warningDelay = 30;
    private float prevWarning;

    private bool transistionHasPlayed = false;
    private AudioSource myAudioSource;
    private PostProcessingController myController;
    private GameObject target;
    private Vector3 startPosition;

    // Use this for initialization
    void Start () {
        myAudioSource = GetComponent<AudioSource>();
        myController = GetComponent<PostProcessingController>();
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update () {
        //Manage the global shader.
        Shader.SetGlobalVector("_TargetPos", target.transform.position + new Vector3(40, 0, 0));
        if (target != null) {
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.transform.position.x, target.transform.position.z)) <= minVignetteDistance)
            {
                myController.vignette.intensity = minVignetteValue;
            }
            else if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.transform.position.x, target.transform.position.z)) >= maxVignetteDistance)
            {
                myController.vignette.intensity = maxVignetteValue;
            } else
            {
                myController.vignette.intensity = minVignetteValue - ((minVignetteValue - maxVignetteValue) * ((Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.transform.position.x, target.transform.position.z)) - minVignetteDistance) / (maxVignetteDistance - minVignetteDistance)));
            }

            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.transform.position.x, target.transform.position.z)) <= maxCameraShakeDistance)
            {
                cameraShake(maxCameraShake * (1 - ((Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.transform.position.x, target.transform.position.z)) - minCameraShakeDistance) / (maxCameraShakeDistance - minCameraShakeDistance))));
            }

            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.transform.position.x, target.transform.position.z)) <= warningDistance)
            {
                if (shouldWarn && !GameObject.Find("VoiceLinePlayer").GetComponent<VoiceLinePlayer>().GetIsPlaying() && !GameManager.Consuming)
                {
                    if (Time.time - prevWarning >= warningDelay)
                    {
                        GameObject.Find("VoiceLinePlayer").GetComponent<VoiceLinePlayer>().PlayVoiceLine(1);
                        shouldWarn = false;
                        prevWarning = Time.time;
                    }
                }
            }
            else
            {
                shouldWarn = true;
            }
        }
        if (GameManager.GameOver && !transistionHasPlayed)
        {
            myAudioSource.Play();
            transistionHasPlayed = true;
        }
    }

    public void SetTarget(GameObject obj)
    {
        target = obj;
    }

    private void cameraShake(float intensity)
    {
        transform.localPosition = startPosition + new Vector3(Random.Range(-1, 2) * intensity, Random.Range(-1, 2) * intensity, 0);
    }
}
