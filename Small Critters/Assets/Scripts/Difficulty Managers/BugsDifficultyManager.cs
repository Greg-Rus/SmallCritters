using UnityEngine;
using System.Collections;
using System;

public class BugsDifficultyManager : MonoBehaviour, IDifficultyScaling
{
    public BeeSectionDifficultyManager beeDifficultyManager;
    public FlyDifficultyManager flyDifficultyManager;
    public FireBeetleDifficultyManager fireBeetleDificultyManager;
    public DifficultyParameter sectionLength;
    public float difficultyPercent = 0f;
    public float difficultyPercentStep = 0.01f;


    public int GetNewBugSectionLength()
    {
        return (int)sectionLength.current;
    }

    public BugType GetBugType()
    {
        if (beeDifficultyManager.IsBeePresent()) return BugType.Bee;
        else if (fireBeetleDificultyManager.IsFireBeetlePresent()) return BugType.FireBeetle;
        else if (flyDifficultyManager.IsFlyPresent()) return BugType.Fly;
        else return BugType.None;

    }

    public void ScaleDifficulty()
    {
        sectionLength.scaleCurrent(difficultyPercent);
        beeDifficultyManager.ScaleDifficulty();
    }
}
