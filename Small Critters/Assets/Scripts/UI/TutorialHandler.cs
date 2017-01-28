using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour {
    public Image tutorialPanel;
    public Sprite[] tutorialScreens;
    public GameObject bonusButton;
    public Text score;
    public Button menuButton;
    private int currentImage = 0;

    public void LoadTutorial()
    {
        menuButton.interactable = false;
        score.text = "77";
        bonusButton.SetActive(false);
        Time.timeScale = 0;
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
        if (currentImage == tutorialScreens.Length - 1)
        {
            score.text = "";
            bonusButton.SetActive(true);
        }
        if (currentImage == tutorialScreens.Length)
        {
            ExitTutorial();
        }
        DisplayTutorialImage();
    }

    private void ExitTutorial()
    {
        menuButton.interactable = true;
        Time.timeScale = 1;
        currentImage = 0;
        this.gameObject.SetActive(false);
    }
}
