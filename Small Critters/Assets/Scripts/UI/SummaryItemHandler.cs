using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SummaryItemHandler : MonoBehaviour {
    public Text myText;
    public Text myCount;
    private int count;
    public Text myPoints;
    private int points;
    public RectTransform myRect;
    public SummaryMenuController myController;
	// Use this for initialization

    public void SetText(string text)
    {
        myText.text = text;
    }
    public void SetCount(int count)
    {
        this.count = count;
        myCount.text = "x" + count.ToString();
    }
    public void SetPoints(int points)
    {
        this.points = points;
        myPoints.text = points.ToString();
    }
    public void SetPositionY(float y)
    {
        Vector3 newPosition = myRect.localPosition;
        newPosition.y = y;
        myRect.localPosition = newPosition;
    }

    public void OnAnimationFinished()
    {
        myController.OnSummaryScoreVisible(points * count) ;
    }
}
