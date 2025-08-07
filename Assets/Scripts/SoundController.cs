using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource src;
    public AudioClip damageSfx, buySfx, buffSfx, clickSfx, elevatorSfx, healSfx, playCardSfx, dealCardSfx, winSfx, defeatSfx;
    private int volume;

    private void Start()
    {
        volume = GameController.volume;
        //src = GetComponent<AudioSource>();
        src.volume = volume;
    }

    public void ClickSound()
    {
        src.volume = GameController.volume;
        src.clip = clickSfx;
        src.Play();
    }

    public void DamageSound()
    {
        src.volume = GameController.volume/2f;
        src.clip = damageSfx;
        src.Play();
    }

    public void BuffSound()
    {
        src.volume = GameController.volume;
        src.clip = buffSfx;
        src.Play();
    }

    public void BuySound()
    {
        src.volume = GameController.volume;
        src.clip = buySfx;
        src.Play();
    }

    public void ElevatorSound()
    {
        src.volume = (float)GameController.volume/2;
        src.clip = elevatorSfx;
        src.Play();
    }

    public void HealSound()
    {
        src.volume = GameController.volume;
        src.clip = healSfx;
        src.Play();
    }

    public void PlayCardSound()
    {
        src.volume = GameController.volume;
        src.clip = playCardSfx;
        src.Play();
    }

    public void DealCardSound()
    {
        src.volume = GameController.volume;
        src.clip = dealCardSfx;
        src.Play();
    }

    public void WinSound()
    {
        src.volume = GameController.volume;
        src.clip = winSfx;
        src.Play();
    }

    public void DefeatSound()
    {
        src.volume = GameController.volume;
        src.clip = defeatSfx;
        src.Play();
    }

}
