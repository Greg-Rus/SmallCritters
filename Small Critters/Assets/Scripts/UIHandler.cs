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
    public GameObject tutorialPanel;
    public GameObject inputTutorial;
    public Text scoreField;
    public HighScoreButtonState lastRunScoreButton;
    public HighScoreButtonState[] scoreButtons;
    private IScoreForUI scoreHandler;
    public Toggle randomToggle;
    public Toggle seededToggle;
    public Toggle swipeUpToggle;
    public Toggle swipeDownToggle;
    //public Toggle showTutorialToggle;

    public bool isMenuContext = false;
    public InputField seedInput;

    public MenuLevel activeMenu;
    public Image heart;
    public float heartFillSpeed;
    private float heartTargetFill;

    public Image powerup;
    public float powerupFillSpeed;
    private float powerupTargetFill;

    private TutorialHandler tutorialHandler;

    private ScoreData scoreData;
    public delegate void ActionSequence();
    private ActionSequence inputChecks;
    public float inactivityTimeToMovementTutorial = 5f;
    public Text ammoCount;
    public Action<SwipeDirection> OnSwipeDirectionChange;
    public GameObject bonusButton;
    private Button bonusButtonScript;
    public GameObject bonusMenu;
    public GameObject newGameMenu;
    public GameObject runSummaryMenu;
    public SummaryMenuController runSummaryConroller;
    public Button customGameButton;
    public Toggle music;
    public Toggle soundFX;
    public ParticleSystem heartFilledEffect;

    private AdHandler myAds;
    private float powerupIconFillTarget;
    private bool isPowerupIconUpdating = false;
    private float heartIconFillTarget;
    private bool isHeartIconUpdating = false;
    private IAudio mySoundFX;
    private Stack<GameObject> menuStack;
    private Vector3 startPointerScreenPositoin;
    private bool showingRunSummary = false;

    // Use this for initialization
    void Start () {
        UpdateUIScore("");
        menuStack = new Stack<GameObject>();
        scoreHandler = ServiceLocator.getService<IScoreForUI>();
        mySoundFX = ServiceLocator.getService<IAudio>();
        myAds = GetComponent<AdHandler>();
        tutorialHandler = tutorialPanel.GetComponent<TutorialHandler>();
        bonusButtonScript = bonusButton.GetComponentInChildren<Button>(true);
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
        if (!isMenuContext && scoreField.text != "")
        {
            inputChecks -= CheckForFirstRowReached;
            bonusButton.SetActive(false);
        }
    }

    private void CheckIdleTime()
    {
        if (Time.timeSinceLevelLoad > inactivityTimeToMovementTutorial)
        {
            inputChecks -= CheckIdleTime;
            //inputChecks -= CheckIfStoppedIdling;
            inputTutorial.SetActive(true);
        }
    }
    private void CheckIfStoppedIdling()
    {
        if (Input.anyKeyDown)
        {
            inputChecks -= CheckIdleTime;
            inputChecks -= CheckIfStoppedIdling;
            inputTutorial.SetActive(false);
        }
    }
    private void CheckIfTappedInstedOfDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPointerScreenPositoin = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            var deltaDrag = Input.mousePosition - startPointerScreenPositoin;
            if (deltaDrag.sqrMagnitude < 0.5f)
            {
                if (showingRunSummary)
                {
                    scoreHandler.SpeedUpSummary();
                }
                else
                {
                    scoreHandler.RestartGame();
                }
            }
        }
    }

    private void CheckForQuitButtonPress()
    {
        if (Input.GetButtonDown("Cancel") && (!quitPrompt.activeSelf))
        {
            OnMenuQuitPrompt();
        }
    }

    private void RestoreMenuState()
    {
        if (PlayerPrefs.GetString("GameMode") == "Seeded")
        {
            seedInput.text = PlayerPrefs.GetString("Seed");
            customGameButton.interactable = true;
			Utilities.PropagateButtonStateToChildren(customGameButton);
		}
        else
        {
			customGameButton.interactable = false;
            Utilities.PropagateButtonStateToChildren(customGameButton);
		}


        if (PlayerPrefs.GetInt("SwipeControlls") == (int)SwipeDirection.Forward)
        {
            swipeUpToggle.isOn = true;
            swipeDownToggle.isOn = false;
        }
        else if (PlayerPrefs.GetInt("SwipeControlls") == (int)SwipeDirection.Backward)
        {
            swipeUpToggle.isOn = false;
            swipeDownToggle.isOn = true;
        }
        else
        {
            PlayerPrefs.SetInt("SwipeControlls", (int)SwipeDirection.Forward);
            swipeUpToggle.isOn = true;
            swipeDownToggle.isOn = false;
        }

        if (PlayerPrefs.GetInt("Music") == (int)Toggled.On)
        {
            music.isOn = true;
        }
        else if (PlayerPrefs.GetInt("Music") == (int)Toggled.Off)
        {
            music.isOn = false;
        }
        else
        {
            PlayerPrefs.SetInt("Music", (int)Toggled.On);
            music.isOn = true;
        }

        if (PlayerPrefs.GetInt("SoundFX") == (int)Toggled.On)
        {
            soundFX.isOn = true;
        }
        else if (PlayerPrefs.GetInt("SoundFX") == (int)Toggled.Off)
        {
            soundFX.isOn = false;
        }
        else
        {
            PlayerPrefs.SetInt("SoundFX", (int)Toggled.On);
            soundFX.isOn = true;
        }

        //if (PlayerPrefs.GetInt("showTutorial") == (int)Toggled.On)
        //{
        //    showTutorialToggle.isOn = true;
        //    ShowTutorial();
        //}
        //else
        //{
        //    showTutorialToggle.isOn = false;
        //}
    }

    public void OnMenuQuitPrompt()
    {
        if (isMenuContext)
        {
            SetActiveToAllOpenMenus(false);
        }
        //else EnableMenuContext();
        OpenMenu(quitPrompt);
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
        SetActiveToAllOpenMenus(false);
        menuStack.Clear();
    }


    public void OnMenuQuitGame()
    {
        //PlayerPrefs.SetInt("LastGameDay", System.DateTime.Today.DayOfYear);
        ReportSessionSummary();
        Application.Quit();
    }

    private void ReportSessionSummary()
    {
        Dictionary<string, object> sessionSummary = new Dictionary<string, object>();
        sessionSummary.Add("ConsecutiveRuns", SessionStatistics.consecutiveRuns);
        sessionSummary.Add("TotalTimePlayed", SessionStatistics.totalTimePlayed);
        UnityEngine.Analytics.Analytics.CustomEvent("SessionSummary", sessionSummary);
    }

    public void OnMenuMain()
    {
        //if (!isMenuContext) EnableMenuContext();
        OpenMenu(mainMenu);
    }

    public void OnMenuHighScores()
    {
        OpenMenu(highScoresMenu);
        UpdateHighScoresMenu();
    }
    public void OnMenuOptions()
    {
        OpenMenu(optionsMenu);
    }

    public void OnNewGameMenu()
    {
        OpenMenu(newGameMenu);
    }

    private void SetActiveToAllOpenMenus(bool state)
    {
        if (menuStack.Count > 0)
        {
            foreach (var menu in menuStack)
            {
                menu.SetActive(state);
            }
        }
        
    }
    private void OpenMenu(GameObject menu)
    {
        if (!isMenuContext) EnableMenuContext();
        menu.SetActive(true);
        menuStack.Push(menu);
    }
    private void CloseMenu(GameObject menu)
    {
        menu.SetActive(false);
        menuStack.Pop();
        menuStack.Peek().SetActive(true);
    }

    public void OnMenuBack()
    {
       
        if (menuStack.Count > 1 && menuStack.Peek() == quitPrompt)
        {
            SetActiveToAllOpenMenus(true);
            CloseMenu(menuStack.Peek());
        }
        else if (menuStack.Count > 1 && menuStack.Peek() == tutorialPanel)
        {
            tutorialHandler.ResetUIState();
            CloseMenu(menuStack.Peek());
        }
        else if (menuStack.Count > 1)
        {
            CloseMenu(menuStack.Peek());
        }
        else
        {
            DisableMenuContext();
        }
        
        
        
    }

    public void UpdateUIScore(int newSocore)
    {
        scoreField.text = newSocore.ToString();
    }
    public void UpdateUIScore(string  newSocore)
    {
        scoreField.text = newSocore;
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

    public void ToggleSwipeUpControlls()
    {
        PlayerPrefs.SetInt("SwipeControlls", (int)SwipeDirection.Forward);
        if (OnSwipeDirectionChange != null)  OnSwipeDirectionChange(SwipeDirection.Forward);
    }
    public void ToggleSwipeDwonControlls()
    {
        PlayerPrefs.SetInt("SwipeControlls", (int)SwipeDirection.Backward);
        if (OnSwipeDirectionChange != null) OnSwipeDirectionChange(SwipeDirection.Backward);
    }

    public void OnSeedEntered()
    {
        PlayerPrefs.SetString("GameMode", "Seeded");
        PlayerPrefs.SetString("Seed", seedInput.text);
		if (seedInput.text != "")
		{
			customGameButton.interactable = true;
		}
		else
		{
			customGameButton.interactable = false;
		}
        Utilities.PropagateButtonStateToChildren(customGameButton);
	}

    public void UpdateHearts(float amount)
    {
        if (amount <= 1f)
        {
            heartTargetFill = amount;
        }
        if (!isHeartIconUpdating)
        {
            StartCoroutine(RadialFillImage(heart, heartTargetFill, heartFillSpeed, isHeartIconUpdating));
        }
    }

    public void UpdatePowerup(float amount)
    {
        
        if (amount <= 1f)
        {
            powerupTargetFill = amount;
        }
        if (!isPowerupIconUpdating)
        {
            StartCoroutine(RadialFillImage(powerup, powerupTargetFill, powerupFillSpeed, isPowerupIconUpdating));
        }
    }

    private IEnumerator RadialFillImage(Image image, float targetFill, float fillSpeed, bool isWorkingFlag)
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
        isWorkingFlag = false;
        if (image.fillAmount == 1f && image == heart)
        {
                heartFilledEffect.Play();
            mySoundFX.PlaySound(Sound.FullHeart);
        }
    }

    //public void OnToggledTutorial()
    //{
    //    PlayerPrefs.SetInt("showTutorial", (int)Toggled.On);
    //}

    public void ShowTutorial()
    {
        OpenMenu(tutorialPanel);
        tutorialHandler.LoadTutorial();
        //showTutorialToggle.isOn = false;
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
        OpenMenu(bonusMenu);
    }

    public void WatchAd()
    {
#if UNITY_EDITOR
        SetActiveToAllOpenMenus(false);
        AdWatched();

#else
        SetActiveToAllOpenMenus(false);
        myAds.ShowInterstitialAd();
#endif


    }
    public void WatchAdLater()
    {
        SetActiveToAllOpenMenus(false);
        DisableMenuContext();
    }

    public Button GetBonusButton()
    {
        return bonusButtonScript;
    }

    public void AdWatched()
    {
        DisableMenuContext();
        ServiceLocator.getService<IPowerup>().SetBonus();
    }

    public void ToggleMusic()
    {
        if (music.isOn)
        {
            ServiceLocator.getService<IAudio>().SetMusicOn(true);
            PlayerPrefs.SetInt("Music", (int)Toggled.On);
        }
        else
        {
            ServiceLocator.getService<IAudio>().SetMusicOn(false);
            PlayerPrefs.SetInt("Music", (int)Toggled.Off);
        }
    }
    public void ToggleSoundFX ()
    {
        if (soundFX.isOn)
        {
            ServiceLocator.getService<IAudio>().SetSoundFXOn(true);
            PlayerPrefs.SetInt("SoundFX", (int)Toggled.On);
        }
        else
        {
            ServiceLocator.getService<IAudio>().SetSoundFXOn(false);
            PlayerPrefs.SetInt("SoundFX", (int)Toggled.Off);
        }
    }

    public void AddSummaryItem(string text, int count, int points, bool animate = true)
    {
        if (!isMenuContext)
        {
            showingRunSummary = true;
            EnableMenuContext();
            OpenMenu(runSummaryMenu);
            inputChecks += CheckIfTappedInstedOfDrag;
        }
        runSummaryConroller.DisplaySummaryItem(text, count, points, animate);
    }
    public void OnSummaryCompositionFinished()
    {
        showingRunSummary = false;
    }
}
