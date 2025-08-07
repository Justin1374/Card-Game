using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unit;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class OptionsMenuController : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject howToPlayMenu;
    private GameObject currentMenu;
    private GameObject currentHowToPlayMenu;
    private SoundController soundPlayer;
    public TMP_Text descriptionText;

    private void Start()
    {
        //Find sound controller
        soundPlayer = FindObjectOfType<SoundController>();
    }

    public void openOptionsMenu()
    {
        soundPlayer.ClickSound();
        currentMenu = Instantiate(optionsMenu, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void closeOptionsMenu()
    {
        soundPlayer.ClickSound();
        Destroy(this.gameObject);
    }

    public void openHowToPlayMenu() //put open and cloe options menu in seperate script
    {
        soundPlayer.ClickSound();
        currentHowToPlayMenu = Instantiate(howToPlayMenu, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void muteSound()
    {
        if (GameController.volume > 0)
        {
            GameController.volume = 0;
        }
        else
        {
            GameController.volume = 1;
        }
    }

    public void returnToMainMenu()
    {
        //Reset game data
        GameController.coins = 0;
        GameController.playerLevel = 0;
        GameController.floorLevel = 0;
        GameController.power1 = false;
        GameController.power2 = false;
        GameController.power3 = false;
        GameController.power4 = false;
        GameController.deckExpansionItem = false;
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void MainMenuDescription()
    {
        descriptionText.text = "Return to the main menu screen. All progress will be lost.";
    }

    public void muteDescription()
    {
        descriptionText.text = "Mute/Unmute game volume.";
    }

    public void quitDescription()
    {
        descriptionText.text = "Exit the game.";
    }

    public void howToPlayDescription()
    {
        descriptionText.text = "How to play.";
    }
}
