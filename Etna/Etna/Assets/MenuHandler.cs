using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{

    public GameObject StartScreen;
    public GameObject OptionsScreen;
    public GameObject CreditsScreen;
    public GameObject LevelSelecterScreen;
    private Levels currentSelectedLevel;
    public Image GameTextImage;
    public Image TutorialTextImage;
    public AudioClip ClickAudio;
    public AudioClip SliderAudio;
    public AudioSource AudioSourceEffects;


    public enum Levels
    {
        Tutorial,
        ChapterOne,
    }

    // Use this for initialization
    void Start()
    {
        currentSelectedLevel = Levels.Tutorial;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DisableAll()
    {
        StartScreen.SetActive(false);
        OptionsScreen.SetActive(false);
        CreditsScreen.SetActive(false);
        LevelSelecterScreen.SetActive(false);
    }

    public void ActivateStartScreen()
    {
        DisableAll();
        StartScreen.SetActive(true);
    }

    public void ActivateOptionsScreen()
    {
        DisableAll();
        OptionsScreen.SetActive(true);
    }

    public void ActivateCreditsScreen()
    {
        DisableAll();
        CreditsScreen.SetActive(true);
    }

    public void ActivateLevelSelectScreen()
    {
        DisableAll();
        LevelSelecterScreen.SetActive(true);
    }

    public void StartSelectedLevel()
    {
        switch (currentSelectedLevel)
        {
            case Levels.Tutorial:
                SceneManager.LoadScene("LevelTutorial", LoadSceneMode.Single);
                break;
            case Levels.ChapterOne:
                SceneManager.LoadScene("All Levels", LoadSceneMode.Single);
                break;
        }
    }

    public void SwitchSelected()
    {
        if (currentSelectedLevel == Levels.Tutorial)
        {
            currentSelectedLevel = Levels.ChapterOne;
            GameTextImage.enabled = true;
            TutorialTextImage.enabled = false;
        }
        else
        {
            currentSelectedLevel = Levels.Tutorial;
            GameTextImage.enabled = false;
            TutorialTextImage.enabled = true;
        }
    }

    public void OnClickAudio()
    {
        if (AudioSourceEffects.clip != ClickAudio)
        {
            AudioSourceEffects.clip = ClickAudio;
        }
        AudioSourceEffects.Play();
    }

    public void OnSlideAudio()
    {
        if (AudioSourceEffects.clip != SliderAudio)
        {
            AudioSourceEffects.clip = SliderAudio;
        }
        if (AudioSourceEffects.isPlaying != true)
        { AudioSourceEffects.Play(); }
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
