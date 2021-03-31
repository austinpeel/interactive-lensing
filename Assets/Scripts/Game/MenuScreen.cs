using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour
{
    // For broadcasting
    public GameEvent OnShowMenu;
    public GameEvent OnHideMenu;
    public GameEvent OnNewGame;

    // Assign in the inspector
    public TextMeshProUGUI counter;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI finalScore;
    public Image heatMeter;
    public Button continueButton;
    public CustomToggle timedGameToggle;

    private Transform backgroundPanel;
    private Transform foregroundPanel;

    private bool menuIsShowing;
    private bool gameInProgress;

    private void Awake()
    {
        backgroundPanel = transform.Find("Background Panel");
        foregroundPanel = transform.Find("Foreground Panel");
    }

    private void Start()
    {
        ShowMenu();
    }

    private void Update()
    {
        if (gameInProgress)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //print("MenuScreen > heard SPACE, toggling menu");
                ToggleMenu();
            }
        }
    }

    private void ToggleMenu()
    {
        if (menuIsShowing)
        {
            HideMenu();
        }
        else
        {
            ShowMenu();
        }
    }

    private void ShowMenu()
    {
        backgroundPanel.gameObject.SetActive(true);
        foregroundPanel.gameObject.SetActive(true);

        menuIsShowing = true;
        Time.timeScale = 0;
        if (OnShowMenu != null)
        {
            //print("MenuScreen > Raising OnShowMenu");
            OnShowMenu.Raise();
        }
    }

    public void HideMenu()
    {
        backgroundPanel.gameObject.SetActive(false);
        foregroundPanel.gameObject.SetActive(false);
        menuIsShowing = false;
        Time.timeScale = 1;
        if (OnHideMenu != null)
        {
            //print("MenuScreen > Raising OnHideMenu");
            OnHideMenu.Raise();
        }
    }

    public void ToggleHeatMeterVisibility()
    {
        heatMeter.gameObject.SetActive(!heatMeter.gameObject.activeInHierarchy);
    }

    public void StartNewGame()
    {
        //print("MenuScreen > StartNewGame (called from button)");
        gameInProgress = true;
        finalScore.gameObject.SetActive(false);
        counter.GetComponent<CounterController>().ResetCounter();
        timer.GetComponent<TimerController>().ResetTimer();
        continueButton.interactable = true;
        //print("MenuScreen > calling HideMenu");
        HideMenu();
    }

    public void TriggerEndGame()
    {
        Cursor.visible = true;
        gameInProgress = false;
        CounterController counterController = counter.GetComponent<CounterController>();
        string lensOrLenses = (counterController.GetCount() == 1) ? " lens!" : " lenses!";
        finalScore.text = "Matched " + counter.text + lensOrLenses;
        finalScore.gameObject.SetActive(true);
        continueButton.interactable = false;
        ShowMenu();
    }

    public void HandleTimedGameToggle()
    {
        if (timedGameToggle != null)
        {
            // Do not allow player to continue game if changing from an untimed game to a timed one
            if (gameInProgress && timedGameToggle.isOn && continueButton.interactable)
            {
                continueButton.interactable = false;
                counter.GetComponent<CounterController>().ResetCounter();
            }
        }
    }
}
