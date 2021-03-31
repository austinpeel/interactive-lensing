using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchScreenController : MonoBehaviour
{
    // Assign in the inspector
    public Image textPanel;
    public TextMeshProUGUI text;

    private float panelStartAlpha = 0.4f;
    private float textStartAlpha = 1;
    private bool fadeIsFinished;

    private void Awake()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, textStartAlpha);
        textPanel.color = new Color(textPanel.color.r, textPanel.color.g, textPanel.color.b, panelStartAlpha);
        textPanel.gameObject.SetActive(false);
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

        float panelStartValue = textPanel.color.a;
        float textStartValue = text.color.a;

        // Fade loop
        time = 0;
        while (time < duration)
        {
            float newTextAlpha = Mathf.Lerp(textStartValue, 0, time / duration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, newTextAlpha);

            float newTextPanelAlpha = Mathf.Lerp(panelStartValue, 0, time / duration);
            textPanel.color = new Color(textPanel.color.r, textPanel.color.g, textPanel.color.b, newTextPanelAlpha);

            time += Time.deltaTime;
            yield return null;
        }

        fadeIsFinished = true;
        textPanel.gameObject.SetActive(false);
        textPanel.color = new Color(textPanel.color.r, textPanel.color.g, textPanel.color.b, panelStartAlpha);
        text.color = new Color(text.color.r, text.color.g, text.color.b, textStartAlpha);
        //print(fadeIsFinished);
    }

    public void ShowMatchedText()
    {
        textPanel.gameObject.SetActive(true);
        StartCoroutine(LerpFadeOut(0.2f, 2));
    }

    public void HideMatchedText()
    {
        if (textPanel.gameObject.activeInHierarchy)
        {
            textPanel.gameObject.SetActive(false);
        }
        
        if (!fadeIsFinished)
        {
            StopCoroutine(nameof(LerpFadeOut));
            fadeIsFinished = true;
        }
    }
}
