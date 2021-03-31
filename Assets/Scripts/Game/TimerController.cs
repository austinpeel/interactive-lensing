using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public GameEvent OnGameOver;

    private TextMeshProUGUI timerTMP;
    private bool countingDown;
    private int totalTime = 60;  // seconds
    private float timeElapsed = 0;

    private void Awake()
    {
        timerTMP = GetComponent<TextMeshProUGUI>();
        UpdateClockText(totalTime);
        //countingDown = true;
        //print("We are counting down ? " + countingDown);
    }

    private void Update()
    {
        if (countingDown)
        {
            timeElapsed += Time.deltaTime;

            if (Mathf.Floor(timeElapsed) > 0)
            {
                int secondsRemaining = totalTime - Mathf.FloorToInt(timeElapsed);
                UpdateClockText(secondsRemaining);
            }

            if (timeElapsed >= totalTime)
            {
                FreezeCountdown();
                if (OnGameOver != null)
                {
                    OnGameOver.Raise();
                }
            }
        }
    }

    public void ToggleTimerVisibility()
    {
        timerTMP.enabled = !timerTMP.enabled;

        // Reset time if turned off
        if (!timerTMP.enabled)
        {
            ResetTimer();
            FreezeCountdown();
        }
        else
        {
            ResumeCountdown();
        }
    }

    public void FreezeCountdown()
    {
        countingDown = false;
    }

    public void ResumeCountdown()
    {
        if (timerTMP.enabled)
        {
            countingDown = true;
        }
    }

    public void NewGame()
    {
        if (timerTMP.enabled)
        {
            countingDown = true;
            timeElapsed = 0;
        }
    }

    public void ResetTimer()
    {
        UpdateClockText(totalTime);
        timeElapsed = 0;
    }

    private void UpdateClockText(int timeInSeconds)
    {
        float seconds = timeInSeconds % 60;
        float minutes = (timeInSeconds - seconds) / 60;
        timerTMP.text = minutes.ToString() + ":" + seconds.ToString("00");
    }
}
