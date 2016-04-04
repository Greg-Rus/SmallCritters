using UnityEngine;
using System.Collections;
[System.Serializable]
public class Score  {
    public string hash {get; set; }
    public int score { get; set; }

    public Score(string hash, int score)
    {
        this.hash = hash;
        this.score = score;
    }
    public Score()
    {
        hash = "";
        score = 0;
    }


}
