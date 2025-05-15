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
        currentLevel.text = GameController.floorLevel.ToString();
        animator = GetComponent<Animator>();
        transitionCanvas = GameObject.Find("ScreenFadeCanvas");
        transition = transitionCanvas.GetComponent<ScreenTransition>();
        //animator.SetBool("ButtonPressed", false);
    }

    public void buttonPressed()
    {
        currentLevel.text = (GameController.floorLevel + 1).ToString();
        animator.SetBool("ButtonPressed", true);
        StartCoroutine(nextLevel("BattleScene"));
    }

    public void shopButton()
    {
        StartCoroutine(nextLevel("Shop"));
    }

    public void homeButton()
    {
        StartCoroutine(nextLevel("Home"));
    }

    public IEnumerator nextLevel(string scneneToLoad)
    {
        transition.TransitionOut();
        yield return new WaitForSeconds(2.5f);
        //animator.SetBool("ButtonPressed", false);
        SceneManager.LoadSceneAsync(scneneToLoad);
    }

    
}
