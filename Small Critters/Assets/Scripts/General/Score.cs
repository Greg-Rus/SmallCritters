using UnityEngine;
using System.Collections;
[System.Serializable]
public class Score  {
    public string hash;
    public int score;

    public Score(string hash, int score)
    {
        this.hash = hash;
        this.score = score;
    }
}
