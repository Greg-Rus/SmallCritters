using UnityEngine;
using System.Collections;

public class FireBeetleDifficultyManager : MonoBehaviour {

    public DifficultyParameter fireBeetleChance;
    public float difficultyPercent = 0f;
    public float difficultyPercentStep = 0.01f;

    public bool IsFireBeetlePresent()
    {
        return RandomLogger.RollBelowPercent(this, fireBeetleChance.current);
    }

    public void ScaleDifficulty()
    {
        if (difficultyPercent < 1f)
        {
            difficultyPercent += difficultyPercentStep;
            fireBeetleChance.scaleCurrent(difficultyPercent);
        }
    }
}
