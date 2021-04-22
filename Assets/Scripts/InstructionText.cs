using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionText : MonoBehaviour
{
    public Image panel;
    public TextMeshProUGUI tmp;

    private float elapsedTime;
    private bool textIsShowing;
    private bool hasDraggedLens;
    private float waitTime = 12;

    private void Awake()
    {
        StartCoroutine(LerpFade(0, 0));
        textIsShowing = false;
    }

    private void Update()
    {
        if (!hasDraggedLens && !textIsShowing)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > waitTime)
            {
                StartCoroutine(LerpFade(1, 0.4f));
                textIsShowing = true;
            }
        }
    }

    public void HandleLensDrag()
    {
        hasDraggedLens = true;

        if (textIsShowing)
        {
            StartCoroutine(LerpFade(0, 1));
        }
    }

    IEnumerator LerpFade(float targetAlpha, float duration, float startDelay = 0)
    {
        // Start delay loop
        float time = 0;
        while (time < startDelay)
        {
            time += Time.deltaTime;
            yield return null;
        }

        float panelStartValue = panel.color.a;
        float tmpStartValue = tmp.color.a;

        // Fade loop
        time = 0;
        while (time < duration)
        {
            float newPanelAlpha = Mathf.Lerp(panelStartValue, targetAlpha, time / duration);
            float newTMPAlpha = Mathf.Lerp(tmpStartValue, targetAlpha, time / duration);
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, newPanelAlpha);
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, newTMPAlpha);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
