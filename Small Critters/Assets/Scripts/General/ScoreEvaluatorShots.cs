using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEvaluatorShots : MonoBehaviour
{
    public float multiKillTimeFrame;
    public ScoreEvaluator shotsScoreData;
    private int killCount = 0;
    private bool multiKill;


    IEnumerator MultiKilltimer()
    {
        yield return new WaitForSeconds(multiKillTimeFrame);
        multiKill = false;
        killCount = 0;
    }
}
