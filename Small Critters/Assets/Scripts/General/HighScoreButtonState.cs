using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class HighScoreButtonState {
    public Button button;
    public Text hash;
    public Text score;

    public void SetHash(string newHash)
    {
        hash.text = newHash;
    }

    public void SetScore(int newScore)
    {
        score.text = newScore.ToString();
    }

    public void Enebled(bool state)
    {
        button.enabled = state;
    }
}
