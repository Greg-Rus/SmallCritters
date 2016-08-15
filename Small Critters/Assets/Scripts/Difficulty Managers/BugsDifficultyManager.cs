using UnityEngine;
using System.Collections;
using System;

public class BugsDifficultyManager : MonoBehaviour, IDifficultyScaling, IBugsSectionDifficulty
{
    public BeeSectionDifficultyManager beeDifficultyManager;
    public FlyDifficultyManager flyDifficultyManager;
    public FireBeetleDifficultyManager fireBeetleDificultyManager;
    public DifficultyParameter sectionLength;
    public float difficultyPercent { get; private set; }
    public float difficultyPercentStep = 0.01f;


    public int GetNewBugSectionLength()
    {
        return (int)sectionLength.current;
    }

    public BugType GetBugType()
    {
        if (fireBeetleDificultyManager.IsFireBeetlePresent()) return BugType.Bee;
        else if (beeDifficultyManager.IsBeePresent()) return BugType.FireBeetle;
        else if (flyDifficultyManager.IsFlyPresent()) return BugType.Fly;
        else return BugType.None;
    }

    public void ScaleDifficulty()
    {
        if (difficultyPercent < 1f)
        {
            difficultyPercent += difficultyPercentStep;
            sectionLength.scaleCurrent(difficultyPercent);
        }
        beeDifficultyManager.ScaleDifficulty();
        fireBeetleDificultyManager.ScaleDifficulty();
    }
}
