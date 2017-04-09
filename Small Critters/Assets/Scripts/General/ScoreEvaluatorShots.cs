using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEvaluatorShots : MonoBehaviour
{
    public float multiKillTimeFrame;
    public ScoreEvaluator shotsScoreData;
    private int killCount = 0;
    private bool multiKill;
    public ScoreEvent[] scoreEvents { get { return shotsScoreData.scoreEvents; } }

    public int GetScoreForKill(string type)
    {
        StopAllCoroutines();
        multiKill = true;
        ++killCount;
        StartCoroutine(MultiKilltimer());
        if (killCount <= shotsScoreData.scoreEvents.Length)
        {
            return shotsScoreData.GetScoreForKill(killCount.ToString());
        }
        else
        {
            return shotsScoreData.GetScoreForKill(shotsScoreData.scoreEvents.Length.ToString());
        }
        
    }

    public string GetNotificationForKill(string type)
    {
        if (killCount <= shotsScoreData.scoreEvents.Length)
        {
            return shotsScoreData.GetNotificationForKill(killCount.ToString());
        }
        else
        {
            return shotsScoreData.GetNotificationForKill(shotsScoreData.scoreEvents.Length.ToString());
        }
    }

    IEnumerator MultiKilltimer()
    {
        yield return new WaitForSeconds(multiKillTimeFrame);
        multiKill = false;
        killCount = 0;
    }
}
