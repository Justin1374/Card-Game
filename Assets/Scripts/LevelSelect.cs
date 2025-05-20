using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public TMP_Text currentLevel;
    Animator animator;
    private GameObject transitionCanvas;
    private ScreenTransition transition;
    private SoundController soundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        soundPlayer = FindObjectOfType<SoundController>();
        currentLevel.text = GameController.floorLevel.ToString();
        animator = GetComponent<Animator>();
        transitionCanvas = GameObject.Find("ScreenFadeCanvas");
        transition = transitionCanvas.GetComponent<ScreenTransition>();
    }

    public void buttonPressed()
    {
        soundPlayer.ClickSound();
        soundPlayer.ElevatorSound();
        currentLevel.text = (GameController.floorLevel + 1).ToString();
        animator.SetBool("ButtonPressed", true);
        StartCoroutine(nextLevel("BattleScene", 3.5f));
    }

    public void shopButton()
    {
        soundPlayer.ClickSound();
        StartCoroutine(nextLevel("Shop", 1.5f));
    }

    public void homeButton()
    {
        soundPlayer.ClickSound();
        StartCoroutine(nextLevel("Home", 1.5f));
    }

    public IEnumerator nextLevel(string scneneToLoad, float duration)
    {
        transition.TransitionOut(duration);
        yield return new WaitForSeconds(duration);
        SceneManager.LoadSceneAsync(scneneToLoad);
    }

    
}
