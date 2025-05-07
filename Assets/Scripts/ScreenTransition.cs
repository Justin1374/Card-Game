using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration;
    [SerializeField] private bool entry = false;

    public void Start()
    {
        if (entry)
        {
            TransitionIn();
        }
        else
        {
            TransitionOut();
        }
    }

    public void TransitionIn()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0, duration));
    }

    public void TransitionOut()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1, duration));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elaspsedTime = 0.0f;
        while (elaspsedTime < duration)
        {
            elaspsedTime += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elaspsedTime/duration);
            yield return null;
        }
        cg.alpha = end;
    }

}
