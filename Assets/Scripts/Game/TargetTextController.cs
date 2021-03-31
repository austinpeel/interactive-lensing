using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetTextController : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private bool fadeIsFinished;
    private IEnumerator coroutine;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    IEnumerator LerpFadeOut(float duration, float startDelay = 0)
    {
        fadeIsFinished = false;

        // Start delay loop
        float time = 0;
        while (time < startDelay)
        {
            time += Time.deltaTime;
            yield return null;
        }

        float startValue = tmp.color.a;

        // Fade loop
        time = 0;
        while (time < duration)
        {
            float newAlpha = Mathf.Lerp(startValue, 0, time / duration);
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, newAlpha);
            time += Time.deltaTime;
            yield return null;
        }

        fadeIsFinished = true;
        //print(fadeIsFinished);
    }

    public void ShowTargetText()
    {
        //Invoke(nameof(TriggerFadeOut), 2);
        if (!fadeIsFinished && coroutine != null)
        {
            print("Stopping LerpFadeOut");
            StopCoroutine(coroutine);
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 0);
        }
        tmp.color += new Color(0, 0, 0, 1 - tmp.color.a);
        coroutine = LerpFadeOut(1.4f, 2);
        StartCoroutine(coroutine);
    }
}
