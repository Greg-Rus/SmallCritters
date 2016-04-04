using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ScoreData {
    public Score lastRun;
    public List<Score> scores;

    public ScoreData()
    {
        lastRun = new Score("", 0);
        scores = new List<Score>();
    }
}

