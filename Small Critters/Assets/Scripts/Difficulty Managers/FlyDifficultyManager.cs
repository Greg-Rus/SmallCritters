using UnityEngine;
using System.Collections;
using System;

public class FlyDifficultyManager : MonoBehaviour, IDifficultyScaling
{
    public DifficultyParameter flyChance;
    public float difficultyPercent = 0f;
    public float difficultyPercentStep = 0.01f;

    public bool IsFlyPresent()
    {
        return RandomLogger.RollBelowPercent(this, flyChance.current);
    }

    public void ScaleDifficulty()
    {
        if (difficultyPercent < 1f)
        {
            difficultyPercent += difficultyPercentStep;
            flyChance.scaleCurrent(difficultyPercent);
        }
    }
}


