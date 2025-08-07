using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private SoundController soundPlayer;
    //private MusicController musicPlayer;

    private void Start()
    {
        soundPlayer = FindObjectOfType<SoundController>();
        //musicPlayer = FindObjectOfType<MusicController>();
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
