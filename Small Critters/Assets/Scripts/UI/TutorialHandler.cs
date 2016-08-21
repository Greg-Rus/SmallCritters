using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour {
    public Image movementTutImageRight;
    public Image movementTutImageLeft;
    public Sprite[] tutorialSpritesLeft;
    public Sprite[] tutorialSpritesRight;
    public float imageDisplayTime = 1f;
    private float nextImgageTime = 0f;
    private int currentImage = 0;

	void LateUpdate () {
        DisplayTutorial();
	}

    public void LoadTutorial()
    {
        currentImage = 0;
        movementTutImageRight.sprite = tutorialSpritesRight[currentImage];
        movementTutImageLeft.sprite = tutorialSpritesLeft[currentImage];
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
        if (currentImage > tutorialSpritesLeft.Length - 1) currentImage = 0;
        movementTutImageRight.sprite = tutorialSpritesRight[currentImage];
        movementTutImageLeft.sprite = tutorialSpritesLeft[currentImage];
    }

    private void UpdateNextImageTime()
    {
        nextImgageTime += imageDisplayTime;
    }
}
