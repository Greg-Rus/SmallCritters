using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour {
    public Image tutorialPanel;
    public Sprite[] tutorialScreens;
    public GameObject bonusButton;
    public Text score;
    public Button menuButton;
    public UIHandler myUI;
    public int currentImage = 0;

    public void LoadTutorial()
    {
        myUI.UpdateUIScore(77);
        myUI.SetBonusButtonActive(false);
        currentImage = 0;
        DisplayTutorialImage();
    }

    private void DisplayTutorialImage()
    {
        tutorialPanel.sprite = tutorialScreens[currentImage];
    }

    public void OnNext()
    {
        ++currentImage;
        if (currentImage == tutorialScreens.Length - 2)
        {
            //score.text = "";
            myUI.UpdateUIScore("");
            myUI.SetBonusButtonActive(true);
        }
        if (currentImage == tutorialScreens.Length - 1)
        {
            ExitTutorial();
        }
        DisplayTutorialImage();
    }

    private void ExitTutorial()
    {
        PlayerPrefs.SetInt("showTutorial", (int)Toggled.Off);
        myUI.OnMenuBack();
    }
}
