using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummaryMenuController : MonoBehaviour {
    public GameObject SummaryItemPrefab;
    public RectTransform ContentRect;
    private float summaryItemHeight;
    private float lastSummaryItemPositionY = 0;
	// Use this for initialization
	void Start () {
        summaryItemHeight = SummaryItemPrefab.GetComponent<RectTransform>().rect.height;
        Debug.Log(lastSummaryItemPositionY);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DisplaySummaryItem(string text, int count, int points, bool animate =true)
    {
        ContentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, summaryItemHeight * 2f);
        GameObject newSummaryItem = Instantiate(SummaryItemPrefab, ContentRect) as GameObject;
        newSummaryItem.transform.SetParent(ContentRect);
        lastSummaryItemPositionY -= summaryItemHeight;
        
        SummaryItemHandler handler = newSummaryItem.GetComponent<SummaryItemHandler>();
        handler.SetText(text);
        handler.SetCount(count);
        handler.SetPoints(points);
        handler.SetPositionY(lastSummaryItemPositionY);
    }
}
