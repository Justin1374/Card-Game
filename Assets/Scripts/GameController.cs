using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //Game Data
    public static int volume = 1;
    public static int coins = 0;
    public static float xp = 0;
    public static int playerLevel = 0;
    public static int floorLevel = 0;
    public static bool power1 = false;
    public static bool power2 = false;
    public static bool power3 = false;
    public static bool power4 = false;
    public static bool deckExpansionItem = false;

    //Settings controls
    public static void muteSound()
    {
        if(volume > 0)
        {
            volume = 0;
        }
        else
        {
            volume = 1;
        }
    }

    public static void returnToMainMenu()
    {
        //Reset game data
        coins = 0;
        playerLevel = 0;
        floorLevel = 0;
        power1 = false;
        power2 = false;
        power3 = false;
        power4 = false;
        deckExpansionItem = false;
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public static void quitGame()
    {
        Application.Quit();
    }

}
