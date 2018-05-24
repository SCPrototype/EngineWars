using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu_Handler : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject OptionScreen;
    public GameObject QuitMenu;
    public GameObject HUD;
    public Image blackPanel;
    private bool shouldFadeOut = true;
    private float fadeSpeed = 1f;
    public static bool isFading;
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
        FadeToBlack(false, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFading) {
            if (shouldFadeOut)
            {
                blackPanel.color = new Color(blackPanel.color.r, blackPanel.color.g, blackPanel.color.b, blackPanel.color.a + (Time.deltaTime / fadeSpeed));
                if (blackPanel.color.a >= 1)
                {
                    //blackPanel.gameObject.SetActive(false);
                    isFading = false;
                }
            }
            else
            {
                blackPanel.color = new Color(blackPanel.color.r, blackPanel.color.g, blackPanel.color.b, blackPanel.color.a - (Time.deltaTime / fadeSpeed));
                if (blackPanel.color.a <= 0)
                {
                    isFading = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.GameOver)
        {
            TogglePauseMenu(!PauseMenu.activeSelf);
        }
        if (GameManager.GameOver && !isFading && !shouldFadeOut)
        {
            FadeToBlack(true, 0.5f);
            gameOverScreenRequestTime = Time.time;
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
        QuitMenu.SetActive(false);
        HUD.SetActive(!toggle);
        PauseMenu.SetActive(toggle);
        Paused = toggle;
    }

    public void FadeToBlack(bool toggle, float time)
    {
        fadeSpeed = time;
        shouldFadeOut = toggle;
        if (shouldFadeOut)
        {
            blackPanel.color = new Color(blackPanel.color.r, blackPanel.color.g, blackPanel.color.b, 0);
        } else
        {
            blackPanel.color = new Color(blackPanel.color.r, blackPanel.color.g, blackPanel.color.b, 1);
        }
        isFading = true;
    }

    public void OpenOptionMenu()
    {
        PauseMenu.SetActive(false);
        OptionScreen.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void StartRealLevel()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitLevel()
    {
        PauseMenu.SetActive(false);
        QuitMenu.SetActive(true);

    }

    public void BackToMain()
    {
        SceneManager.LoadScene("AllMenuScenes");
    }

    public void BackToPause()
    {
        OptionScreen.SetActive(false);
        PauseMenu.SetActive(true);
        Paused = true;
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
