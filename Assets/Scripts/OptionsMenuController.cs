using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuController : MonoBehaviour
{
    public GameObject optionsMenu;
    private GameObject currentMenu;
    private SoundController soundPlayer;

    private void Start()
    {
        soundPlayer = FindObjectOfType<SoundController>();
    }

    public void openOptionsMenu() //put open and cloe options menu in seperate script
    {
        soundPlayer.ClickSound();
        currentMenu = Instantiate(optionsMenu, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void closeOptionsMenu()
    {
        soundPlayer.ClickSound();
        Destroy(this.gameObject);
    }
}
