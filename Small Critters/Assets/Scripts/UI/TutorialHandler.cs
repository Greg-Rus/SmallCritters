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
    public int nextImage = 0;
    public Button backButton;

    public void LoadTutorial()
    {
        myUI.UpdateUIScore(77);
        myUI.bonusButton.SetActive(false);
        currentImage = 0;
        nextImage = 0;
        DisplayTutorialImage();
    }

    private void DisplayTutorialImage()
    {
        tutorialPanel.sprite = tutorialScreens[currentImage];
        myUI.GetBonusButton().interactable = false;
    }

    public void OnNext()
    {
        ++nextImage;
        if (nextImage == 1)
        {
            backButton.interactable = true;
            Utilities.PropagateButtonStateToChildren(backButton);
        }
        ChangeImage();
    }

    public void OnBack()
    {
        --nextImage;
        if (nextImage == 0)
        {
            backButton.interactable = false;
            Utilities.PropagateButtonStateToChildren(backButton);
        }
        ChangeImage();
    }

    private void ChangeImage()
    {
        if (nextImage == 5 && currentImage < nextImage)
        {
            myUI.UpdateUIScore("");
            myUI.bonusButton.SetActive(true);
        }

        if (nextImage == 4 && currentImage > nextImage)
        {
            myUI.UpdateUIScore(77);
            myUI.bonusButton.SetActive(false);
        }

        if (nextImage == tutorialScreens.Length)
        {
            ExitTutorial();
            return;
        }
        currentImage = nextImage;
        DisplayTutorialImage();
    }

    public void ResetUIState()
    {
        myUI.UpdateUIScore("");
        myUI.bonusButton.SetActive(true);
        myUI.GetBonusButton().interactable = true;
    }

    private void ExitTutorial()
    {
        myUI.OnMenuBack();
        myUI.UpdateUIScore("");
        myUI.GetBonusButton().interactable = true;
    }
}
