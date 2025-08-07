using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource src;
    public AudioClip homeMusic;
    private int volume;

    // Start is called before the first frame update
    void Start()
    {
        volume = GameController.volume;
        src.volume = volume;
    }

    public void playHomeMusic()
    {
        src.volume = GameController.volume;
        src.clip = homeMusic;
        src.Play();
    }
}
