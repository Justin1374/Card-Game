using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private SoundController soundPlayer;

    private void Start()
    {
        soundPlayer = FindObjectOfType<SoundController>();
    }

    public void PlayGame()
    {
        soundPlayer.ClickSound();
        SceneManager.LoadSceneAsync("Home");
    }

    public void Quit()
    {
        soundPlayer.ClickSound();
        Application.Quit();
    }

}
