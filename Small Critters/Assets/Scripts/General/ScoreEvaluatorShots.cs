using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEvaluatorShots : MonoBehaviour
{
    public float multiKillTimeFrame;
    public ScoreEvaluator shotsScoreData;
    private int killCount = 0;
    public ScoreEvent[] scoreEvents { get { return shotsScoreData.scoreEvents; } }

    public int GetScoreForEvent(string type)
    {
        StopAllCoroutines();
        ++killCount;
        StartCoroutine(MultiKilltimer());
        if (killCount <= shotsScoreData.scoreEvents.Length)
        {
            return shotsScoreData.GetScoreForEvent(killCount.ToString());
        }
        else
        {
            return shotsScoreData.GetScoreForEvent(shotsScoreData.scoreEvents.Length.ToString());
        }
        
    }

    public string GetNotificationForEvent(string type)
    {
        if (killCount <= shotsScoreData.scoreEvents.Length)
        {
            return shotsScoreData.GetNotificationForEvent(killCount.ToString());
        }
        else
        {
            return shotsScoreData.GetNotificationForEvent(shotsScoreData.scoreEvents.Length.ToString());
        }
    }

    IEnumerator MultiKilltimer()
    {
        yield return new WaitForSeconds(multiKillTimeFrame);
        killCount = 0;
    }
}
