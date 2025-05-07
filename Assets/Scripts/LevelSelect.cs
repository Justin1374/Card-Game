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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        transitionCanvas = GameObject.Find("ScreenFadeCanvas");
        transition = transitionCanvas.GetComponent<ScreenTransition>();
    }

    public void buttonPressed()
    {
        currentLevel.text = "2";
        animator.SetBool("ButtonPressed", true);
        StartCoroutine(nextLevel());
    }

    public void shopButton()
    {
        
    }

    public IEnumerator nextLevel()
    {
        transition.TransitionOut();
        yield return new WaitForSeconds(3.0f);
        animator.SetBool("ButtonPressed", false);
        SceneManager.LoadSceneAsync("BattleScene");
    }

    
}
