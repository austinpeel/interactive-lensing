using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderPulsate : MonoBehaviour
{
    public Image image;
    public Color color1 = Color.white;
    public Color color2 = Color.white;
    [Min(0.01f)]
    public float frequency = 1;  // pulses per second
    public float startDelay;

    private float halfPeriod;
    private Color currentStartColor;
    private Color currentTargetColor;
    private bool pulsating = false;
    private bool hasClicked = false;
    private float elapsedTime;

    private void Awake()
    {
        halfPeriod = 1 / (2 * frequency);
        image.color = color1;
        currentStartColor = image.color;
        currentTargetColor = color2;
    }

    private void Update()
    {
        if (pulsating)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / halfPeriod);
            t = t * t * (3f - 2f * t);
            image.color = Color.Lerp(currentStartColor, currentTargetColor, t);

            if (t == 1)
            {
                elapsedTime = 0;
                ToggleTargetColor();
            }
        }
        else if (!hasClicked)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > startDelay)
            {
                elapsedTime = 0;
                pulsating = true;
            }
        }
    }

    private void ToggleTargetColor()
    {
        //currentTargetColor = (currentTargetColor == color1) ? color2 : color1;
        Color temp = currentStartColor;
        currentStartColor = currentTargetColor;
        currentTargetColor = temp;
    }

    public void DoNotPulsate()
    {
        hasClicked = true;
        pulsating = false;
        image.color = color1;
    }
}
