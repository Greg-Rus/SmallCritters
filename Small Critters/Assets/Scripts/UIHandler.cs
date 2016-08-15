using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public enum MenuLevel {MenuOff, MenuBackground, MainMenu, SubMenu, QuitPrompt, Bonus };
public class UIHandler : MonoBehaviour {
    public GameObject pausePanel;
    public GameObject quitPrompt;
    public GameObject mainMenu;
    public GameObject highScoresMenu;
    public GameObject optionsMenu;
    public Text scoreField;
    public HighScoreButtonState lastRunScoreButton;
    public HighScoreButtonState[] scoreButtons;
    private IScoreForUI scoreHandler;
    public Toggle randomToggle;
    public Toggle seededToggle;
    public Toggle swipeUpToggle;
    public Toggle swipeDownToggle;

    public bool isMenuContext = false;
    public InputField seedInput;

    public MenuLevel activeMenu;
    public Image heart;
    public float heartFillSpeed;
    private float heartTargetFill;

    public Image powerup;
    public float powerupFillSpeed;
    private float powerupTargetFill;

    public TutorialHandler tutorialHandler;

    private GameObject lastMenu;
    private MenuLevel lastMenuLevel;
    private GameObject currentMenu;
    private MenuLevel currentMenuLevel;
    private ScoreData scoreData;
    public delegate void ActionSequence();
    private ActionSequence inputChecks;
    public float inactivityTimeToMovementTutorial = 5f;
    public Text ammoCount;
    public Action<float> OnSwipeDirectionChange;
    public GameObject bonusButton;
    public GameObject bonusMenu;
    public GameObject newGameMenu;
    public Button customGameButton;

    private AdHandler myAds;

    // Use this for initialization
    void Start () {
        scoreHandler = ServiceLocator.getService<IScoreForUI>();
        myAds = GetComponent<AdHandler>();
        Time.timeScale = 1;
        RestoreMenuState();
        inputChecks += CheckForQuitButtonPress;
        inputChecks += CheckIdleTime;
        inputChecks += CheckIfStoppedIdling;
        inputChecks += CheckForFirstRowReached;
    }
	
	// Update is called once per frame
	void Update ()
    {
        inputChecks();
	}

    private void CheckForFirstRowReached()
    {
        if (scoreField.text != "")
        {
            inputChecks -= CheckForFirstRowReached;
            DisableBonusButton();
        }
    }

    private void CheckIdleTime()
    {
        if (Time.timeSinceLevelLoad > inactivityTimeToMovementTutorial)
        {
            inputChecks -= CheckIdleTime;
            ShowTutorial();
        }
    }
    private void CheckIfStoppedIdling()
    {
        if (Input.anyKeyDown)
        {
            inputChecks -= CheckIdleTime;
        }
    }

    private void CheckForQuitButtonPress()
    {
        if (Input.GetButtonDown("Cancel") && (currentMenuLevel != MenuLevel.QuitPrompt))
        {
            OnMenuQuitPrompt();
        }
    }
    private void CheckForTutorialDismissal()
    {
        if (Input.anyKeyDown)
        {
            DismissTutorial();
            inputChecks -= CheckForTutorialDismissal;
        }
    }

    private void DismissTutorial()
    {
        tutorialHandler.gameObject.SetActive(false);
    }

    private void RestoreMenuState()
    {
        if (PlayerPrefs.GetString("GameMode") == "Seeded")
        {
            seedInput.text = PlayerPrefs.GetString("Seed");
            customGameButton.interactable = true;
			PropagateButtonStateToChildren(customGameButton);
		}
        else
        {
			customGameButton.interactable = false;
			PropagateButtonStateToChildren(customGameButton);
		}


        if (PlayerPrefs.GetFloat("SwipeControlls") == (float)SwipeDirection.Forward)
        {
            swipeUpToggle.isOn = true;
            swipeDownToggle.isOn = false;
        }
        else if (PlayerPrefs.GetFloat("SwipeControlls") == (float)SwipeDirection.Backward)
        {
            swipeUpToggle.isOn = false;
            swipeDownToggle.isOn = true;
        }
        else
        {
            PlayerPrefs.SetFloat("SwipeControlls", (float)SwipeDirection.Forward);
            swipeUpToggle.isOn = true;
            swipeDownToggle.isOn = false;
        }
    }

	public void PropagateButtonStateToChildren(Button button)
	{
		Image[] images = button.GetComponentsInChildren<Image>();
		Color targetColor = (button.interactable) ? button.colors.normalColor : button.colors.disabledColor;
		foreach (Image image in images)
		{
			image.color = targetColor;
		}
	}

    public void OnMenuQuitPrompt()
    {
        if (isMenuContext)
        {
            SaveLastMenu(currentMenu, currentMenuLevel);
            HideCurrentMenu();
        }
        else EnableMenuContext();

        SetCurrentMenu(quitPrompt, MenuLevel.QuitPrompt);
        ShowCurrentMenu();
    }



    public void EnableMenuContext()
    {
        isMenuContext = true;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }
    private void DisableMenuContext()
    {
        isMenuContext = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        currentMenu = lastMenu = null;
        lastMenuLevel = currentMenuLevel = MenuLevel.MenuOff;
    }


    public void OnMenuQuitGame()
    {
        PlayerPrefs.SetInt("LastGameDay", System.DateTime.Today.DayOfYear);
        Application.Quit();
    }

    public void OnMenuMain()
    {
        if (!isMenuContext) EnableMenuContext();
        SetCurrentMenu(mainMenu, MenuLevel.MainMenu);
        ShowCurrentMenu();
    }

    public void OnMenuHighScores()
    {
        SaveLastMenu(currentMenu, currentMenuLevel);
        SetCurrentMenu(highScoresMenu, MenuLevel.SubMenu);
        ShowCurrentMenu();
        UpdateHighScoresMenu();
    }
    public void OnMenuOptions()
    {
        SaveLastMenu(currentMenu, currentMenuLevel);
        SetCurrentMenu(optionsMenu, MenuLevel.SubMenu);
        ShowCurrentMenu();
    }

    public void OnNewGameMenu()
    {
        SaveLastMenu(currentMenu, currentMenuLevel);
        SetCurrentMenu(newGameMenu, MenuLevel.SubMenu);
        ShowCurrentMenu();
    }

    private void ShowCurrentMenu()
    {
        currentMenu.SetActive(true);
    }
    private void HideCurrentMenu()
    {
        currentMenu.SetActive(false);
    }

    private void SaveLastMenu(GameObject menu, MenuLevel level)
    {
        lastMenu = menu;
        lastMenuLevel = level;
    }
    private void ShowLastMenu()
    {
        HideCurrentMenu();
        SetCurrentMenu(lastMenu, lastMenuLevel);
        if (currentMenuLevel == MenuLevel.MenuOff)
        {
            DisableMenuContext();
        }
        else ShowCurrentMenu();
    }
    private void SetCurrentMenu(GameObject menu, MenuLevel level)
    {
        currentMenu = menu;
        currentMenuLevel = level;
    }

    public void OnMenuBack()
    {
        switch (currentMenuLevel)
        {
            case MenuLevel.MainMenu: { HideCurrentMenu(); DisableMenuContext(); break; }
            case MenuLevel.QuitPrompt: { ShowLastMenu(); break; }
            case MenuLevel.SubMenu: { HideCurrentMenu(); ShowLastMenu(); break; }
            case MenuLevel.Bonus: { HideCurrentMenu(); DisableMenuContext(); break; }
        }
    }

    public void UpdateUIScore(int newSocore)
    {
        scoreField.text = newSocore.ToString();
    }

    public void UpdateHighScoresMenu()
    {
        scoreData = scoreHandler.GetScoreData();   
        UpdateScoreButton(lastRunScoreButton, scoreData.lastRun);
        for (int i = 0; i < scoreData.scores.Count; ++i)
        {
            UpdateScoreButton(scoreButtons[i], scoreData.scores[i]);
        }
    }
    private void UpdateScoreButton(HighScoreButtonState button, Score scoreEntry)
    {
        button.hash.text = scoreEntry.hash;
        button.score.text = scoreEntry.score.ToString();
        if (scoreEntry.hash == "")
        {
            button.button.interactable = false;
        }
        else
        {
            button.button.interactable = true;
        }
    }

    public void OnRestartRun(int button)
    {
        scoreHandler.RestartRun(button);
    }

    public void ToggleRandomGame()
    {
        if (randomToggle.isOn)
        {
            seedInput.interactable = false;
            PlayerPrefs.SetString("Seed", "");
            PlayerPrefs.SetString("GameMode", "Radom");
        }
        
    }
    public void ToggleSeededGame()
    {
        if (seededToggle.isOn)
        {
            seedInput.interactable = true;
            PlayerPrefs.SetString("GameMode", "Seeded");
        }
       
    }

    public void ToggleSwipeUpControlls()
    {
        PlayerPrefs.SetFloat("SwipeControlls", 1);
        if (OnSwipeDirectionChange != null)  OnSwipeDirectionChange(1);
    }
    public void ToggleSwipeDwonControlls()
    {
        PlayerPrefs.SetFloat("SwipeControlls", -1);
        if (OnSwipeDirectionChange != null) OnSwipeDirectionChange(-1);
    }

    public void OnSeedEntered()
    {
        PlayerPrefs.SetString("Seed", seedInput.text);
		if (seedInput.text != "")
		{
			customGameButton.interactable = true;
		}
		else
		{
			customGameButton.interactable = false;
		}
		PropagateButtonStateToChildren(customGameButton);
	}

    public void UpdateHearts(float amount)
    {
        if (amount <= 1f)
        {
            heartTargetFill = amount;
        }
        StartCoroutine(RadialFillImage(heart, heartTargetFill, heartFillSpeed));
    }

    public void UpdatePowerup(float amount)
    {
        
        if (amount <= 1f)
        {
            powerupTargetFill = amount;
        }
        StartCoroutine(RadialFillImage(powerup, powerupTargetFill, powerupFillSpeed));
    }

    private IEnumerator RadialFillImage(Image image, float targetFill, float fillSpeed)
    {
        if (image.fillAmount < targetFill)
        {
            while (image.fillAmount < targetFill)
            {
                image.fillAmount += Time.deltaTime * fillSpeed;
                yield return null;
            }
        }
        else if (image.fillAmount > targetFill)
        {
            while (image.fillAmount > targetFill)
            {
                image.fillAmount -= Time.deltaTime * fillSpeed * 2;
                yield return null;
            }
        }

    }

    public void ShowTutorial()
    {
        tutorialHandler.gameObject.SetActive(true);
        inputChecks += CheckForTutorialDismissal;
        tutorialHandler.LoadTutorial();
    }

    public void PowerupMode(bool isActive)
    {
        if (isActive)
        {
            ammoCount.enabled = true;
        }
        else
        {
            ammoCount.enabled = false;
            powerup.fillAmount = 0;
        }
    }
    public void UpdateAmmoCount(int ammo)
    {
        ammoCount.text = ammo.ToString();
    }

    public void OnBonusPress()
    {
        bonusButton.SetActive(false);
        if (!isMenuContext) EnableMenuContext();
        SetCurrentMenu(bonusMenu, MenuLevel.MainMenu);
        ShowCurrentMenu();
    }

    public void WatchAd()
    {
        myAds.ShowInterstitialAd();
        HideCurrentMenu();
        
    }
    public void WatchAdLater()
    {
        HideCurrentMenu();
        DisableMenuContext();
    }

    private void DisableBonusButton()
    {
        bonusButton.SetActive(false);
    }

    public void AdWatched()
    {
        DisableMenuContext();
        ServiceLocator.getService<IPowerup>().SetBonus();
    }
}
