using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryItemHandler : MonoBehaviour {
    public Text myText;
    public Text myCount;
    public Text myPoints;
    public RectTransform myRect;
	// Use this for initialization

    public void SetText(string text)
    {
        myText.text = text;
    }
    public void SetCount(float count)
    {
        myCount.text = count.ToString();
    }
    public void SetPoints(float points)
    {
        myPoints.text = points.ToString();
    }
    public void SetPositionY(float y)
    {
        Vector3 newPosition = myRect.localPosition;
        newPosition.y = y;
        myRect.localPosition = newPosition;
    }
}
