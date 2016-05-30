using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public enum MenuLevel {MenuOff, MenuBackground, MainMenu, SubMenu, QuitPrompt };
public class UIHandler : MonoBehaviour {
    public GameObject pausePanel;
    public GameObject quitPrompt;
    public GameObject mainMenu;
    public GameObject highScoresMenu;
    public GameObject optionsMenu;
    public Text scoreField;
    public HighScoreButtonState lastRunScoreButton;
    public HighScoreButtonState[] scoreButtons;
    public ScoreHandler scoreHandler;
    public Toggle randomToggle;
    public Toggle seededToggle;

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
    private Action currentMenuToggle;
    private GameObject currentMenu;
    private MenuLevel currentMenuLevel;
    private ScoreData scoreData;
    public delegate void ActionSequence();
    private ActionSequence inputChecks;
    private bool tutorialActive = false;
    //private bool idling = true;
    public float inactivityTimeToMovementTutorial = 5f;
    public Text ammoCount;

    // Use this for initialization
    void Start () {
        Time.timeScale = 1;
        RestoreMenuState();
        inputChecks += CheckForQuitButtonPress;
        inputChecks += CheckIdleTime;
        inputChecks += CheckIfStoppedIdling;
        //scoreData = scoreHandler.scoreData;
        //Debug.Log(Application.persistentDataPath);
    }
	
	// Update is called once per frame
	void Update () {
        inputChecks();
        //if (Input.GetButtonDown("Cancel") && (currentMenuLevel != MenuLevel.QuitPrompt))
        //{
        //    OnMenuQuitPrompt();
        //}
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
        if (Input.anyKeyDown)// && (tutorialActive))
        {
            inputChecks -= CheckIdleTime;
        }
    }

    private void CheckForQuitButtonPress()
    {
        //Debug.Log("Checking for Quit");
        if (Input.GetButtonDown("Cancel") && (currentMenuLevel != MenuLevel.QuitPrompt))
        {
            OnMenuQuitPrompt();
        }
    }
    private void CheckForTutorialDismissal()
    {
        //Debug.Log("Checking for Dsimissal");
        if (Input.anyKeyDown)// && (tutorialActive))
        {
            DismissTutorial();
            inputChecks -= CheckForTutorialDismissal;
        }
    }

    private void DismissTutorial()
    {
        tutorialHandler.gameObject.SetActive(false);
        tutorialActive = false;
    }

    private void RestoreMenuState()
    {
        if (PlayerPrefs.GetString("GameMode") == "Seeded")
        {
            randomToggle.isOn = false;
            seededToggle.isOn = true;
            seedInput.text = PlayerPrefs.GetString("Seed");
        }
        else
        {
            randomToggle.isOn = true;
            seededToggle.isOn = false;
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
        UpdateOptionsMenu();
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
        }
    }

    public void UpdateUIScore(int newSocore)
    {
        scoreField.text = newSocore.ToString();
    }

    public void UpdateOptionsMenu()
    {

    }

    public void UpdateHighScoresMenu()
    {
        scoreData = scoreHandler.GetScoreData();
        //Debug.Log("Last Run: " + scoreData.lastRun.score);
        //foreach (Score entry in scoreData.scores)
        //{
        //    Debug.Log("Item: " + entry.score);
        //}
        //Debug.Log(lastRunScoreButton.score.text);
        
        UpdateScoreButton(lastRunScoreButton, scoreData.lastRun);
        //Debug.Log(lastRunScoreButton.score.text);
        for (int i = 0; i < scoreData.scores.Count; ++i)
        {
            UpdateScoreButton(scoreButtons[i], scoreData.scores[i]);
        }
        //int listIndex = 0;
        //for (LinkedListNode<Score> node = scoreData.scores.First; node != null; node = node.Next)
        //{
        //    UpdateScoreButton(scoreButtons[listIndex], node.Value);
        //}
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
            //Debug.Log("Toggling Random Game Mode!!");
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
    public void OnSeedEntered()
    {
        //Debug.Log(seedInput.text);
        PlayerPrefs.SetString("Seed", seedInput.text);
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
                image.fillAmount -= Time.deltaTime * fillSpeed;
                yield return null;
            }
        }

    }

    public void ShowTutorial()
    {
        tutorialActive = true;
        tutorialHandler.gameObject.SetActive(true);
        inputChecks += CheckForTutorialDismissal;
        tutorialHandler.LoadTutorial();
    }

    public void PowerupMode(bool isActive)
    {
        if (isActive) ammoCount.enabled = true;
        else ammoCount.enabled = false;
    }
    public void UpdateAmmoCount(int ammo)
    {
        ammoCount.text = ammo.ToString();
    }


}
