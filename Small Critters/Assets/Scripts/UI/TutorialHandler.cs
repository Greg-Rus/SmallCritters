using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour {
    public Image tutorialImage;

    public Sprite[] tutorialSprites;
    public float imageDisplayTime = 1f;
    private float nextImgageTime = 0f;
    private int currentImage = 0;

	void LateUpdate () {
        DisplayTutorial();
	}

    public void LoadTutorial()
    {
        currentImage = 0;
        tutorialImage.sprite = tutorialSprites[currentImage];
        nextImgageTime = Time.timeSinceLevelLoad + nextImgageTime;
    }

    private void DisplayTutorial()
    {
        if (nextImgageTime <= Time.timeSinceLevelLoad)
        {
            SwapTutorialImage();
            UpdateNextImageTime();
        }
    }

    private void SwapTutorialImage()
    {
        ++currentImage;
        if (currentImage > tutorialSprites.Length - 1) currentImage = 0;
        tutorialImage.sprite = tutorialSprites[currentImage];
    }

    private void UpdateNextImageTime()
    {
        nextImgageTime += imageDisplayTime;
    }
}
