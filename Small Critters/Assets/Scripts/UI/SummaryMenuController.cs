using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SummaryMenuController : MonoBehaviour
{
    public GameObject SummaryItemPrefab;
    public RectTransform ContentRect;
    public RectTransform ScrollViewRect;
    public Scrollbar scrollBar;
    public Text finalScore;
    public SoundController myAudio;
    private float summaryItemHeight;
    private float lastSummaryItemPositionY = 0;
    public bool test = false;
    private int lastScoreCount = 0;
    private float scoreCountDuration = 1f;
    
    private Vector3 endPointerScreenPositoin;
    // Use this for initialization
    void Awake ()
    {
        summaryItemHeight = SummaryItemPrefab.GetComponent<RectTransform>().rect.height;
        lastSummaryItemPositionY = summaryItemHeight * 0.5f;
    }


    public void DisplaySummaryItem(string text, int count, int points, bool animate =true)
    {
        lastSummaryItemPositionY -= summaryItemHeight;
        ContentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lastSummaryItemPositionY * -1f + summaryItemHeight * 0.5f);
        GameObject newSummaryItem = Instantiate(SummaryItemPrefab, new Vector3(700f, lastSummaryItemPositionY, 0f), Quaternion.identity) as GameObject;
        newSummaryItem.transform.SetParent(ContentRect.transform, false);
        
        SummaryItemHandler handler = newSummaryItem.GetComponent<SummaryItemHandler>();
        handler.SetText(text);
        handler.SetCount(count);
        handler.SetPoints(points*count);
        handler.myController = this;
        handler.myAudio = myAudio;

        if (ContentRect.rect.height >= ScrollViewRect.rect.height)
        {
            StartCoroutine(ResetScrollBarAtEndOfFrame());
        }

        
        //handler.SetPositionY(lastSummaryItemPositionY);
    }
    IEnumerator ResetScrollBarAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        scrollBar.value = 0f;
    }

    public void OnSummaryScoreVisible(int points)
    {
        lastScoreCount += points;
        finalScore.text = lastScoreCount.ToString();
    }

    //IEnumerator AddToFinalSocreDynamic(int points)
    //{
    //    float pointPool = points;
    //    float startTime = Time.timeSinceLevelLoad;
    //    float endTime = scoreCountDuration + startTime;
    //    float t = 0;
    //    float newPoints = 0;
    //    float oldPoints = 0;

    //    while (t != 1f)
    //    {
    //        oldPoints = newPoints;
    //        t += Time.deltaTime * scoreCountDuration;
    //        newPoints = Mathf.Lerp(0, points, t);
    //        float pointDelta = newPoints - oldPoints;
    //        finalScore.text = ((int)pointDelta).ToString();
    //        yield return null;
    //    }

    //}
}
