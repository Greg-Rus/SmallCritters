using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public enum MenuLevel {MenuOff, MenuBackground, MainMenu, SubMenu, QuitPrompt };
public class UIHandler : MonoBehaviour {
    public GameObject pausePanel;
    public GameObject quitPrompt;
    public GameObject mainMenu;

    public bool isMenuContext = false;

    public MenuLevel activeMenu;
    private GameObject lastMenu;
    private MenuLevel lastMenuLevel;
    private Action currentMenuToggle;
    private GameObject currentMenu;
    private MenuLevel currentMenuLevel;

    // Use this for initialization
    void Start () {
        Time.timeScale = 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Cancel") && (currentMenuLevel != MenuLevel.QuitPrompt))
        {
            OnMenuQuitPrompt();
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
        Application.Quit();
    }

    public void OnMenuMain()
    {
        if (!isMenuContext) EnableMenuContext();
        SetCurrentMenu(mainMenu, MenuLevel.MainMenu);
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
        }
    }


}
