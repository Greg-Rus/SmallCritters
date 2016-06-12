using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ScoreData {
    public Score lastRun {get; set; }
    public List<Score> scores { get; set; }

    public ScoreData()
    {
        lastRun = new Score("", 0);
        scores = new List<Score>();
    }
}

