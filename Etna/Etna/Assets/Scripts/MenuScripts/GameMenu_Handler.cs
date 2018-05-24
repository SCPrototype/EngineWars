using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu_Handler : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject blackPanel;
    public GameObject OptionScreen;
    public GameObject QuitMenu;
    public GameObject HUD;
    public static bool Paused = false;
    private const float timeToGameOverScreen = 2;
    private float gameOverScreenRequestTime;
    public AudioClip ClickAudio;
    public AudioClip SliderAudio;
    public AudioSource AudioSourceEffects;

    // Use this for initialization
    void Start()
    {
        PauseMenu.SetActive(false);
        Paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.GameOver)
        {
            TogglePauseMenu(!PauseMenu.activeSelf);
        }
        if (GameManager.GameOver && !blackPanel.activeSelf)
        {
            FadeToBlack(true);
            gameOverScreenRequestTime = Time.time;
        }
        else if (!GameManager.GameOver && blackPanel.activeSelf)
        {
            FadeToBlack(false);
            gameOverScreenRequestTime = 0;
        }

        if (Time.time - gameOverScreenRequestTime >= GameManager.BlackOutTime && GameManager.GameOver)
        {
            AudioListener.volume = 1;
            GameManager.GameOver = false;
            GameManager.Consuming = false;
            Shader.SetGlobalVector("_TargetPos", new Vector3(0, 0, 0));
            SceneManager.LoadScene("GameOver");
        }
    }

    public void TogglePauseMenu(bool toggle)
    {
        HUD.SetActive(!toggle);
        OptionScreen.SetActive(false);
        QuitMenu.SetActive(false);
        PauseMenu.SetActive(toggle);
        Paused = toggle;
    }

    public void FadeToBlack(bool toggle)
    {
        blackPanel.SetActive(toggle);
    }

    public void OpenOptionMenu()
    {
        PauseMenu.SetActive(false);
        OptionScreen.SetActive(true);
    }

    public void BackToPause()
    {
        OptionScreen.SetActive(false);
        PauseMenu.SetActive(true);
        Paused = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitLevel()
    {
        PauseMenu.SetActive(false);
        QuitMenu.SetActive(true);

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
}
