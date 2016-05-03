using UnityEngine;
using System.Collections;

public class BeeSectionDifficultyManager : MonoBehaviour, IBeeSectionDifficulty, IDifficultyScaling {
	public MainGameController gameController;
	public float difficultyPercent = 0f;
	public float difficultyPercentStep = 0.01f;
	public DifficultyParameter chargeTime;
	public DifficultyParameter flySpeed;
	public DifficultyParameter chargeSpeed;
	public DifficultyParameter chargeDistance;
	public DifficultyParameter sectionLength;
    public DifficultyParameter beeChance;

	public void Start()
	{
		ScaleDifficulty();
	}

	public void ScaleDifficulty()
	{
		if(difficultyPercent< 1f)
		{
			difficultyPercent += difficultyPercentStep;
			chargeTime.scaleCurrent(difficultyPercent);
			flySpeed.scaleCurrent(difficultyPercent);
			chargeSpeed.scaleCurrent(difficultyPercent);
			chargeDistance.scaleCurrent(difficultyPercent);
			sectionLength.scaleCurrent(difficultyPercent);
            beeChance.scaleCurrent(difficultyPercent);
		}
	}
	
	public float GetChargeTime()
	{
		return chargeTime.current;
	}
	public float GetFlySpeed()
	{
		return flySpeed.current;
	}
	public float GetChargeSpeed()
	{
		return chargeSpeed.current;
	}
	public float GetChargeDistance()
	{
		return chargeDistance.current;
	}
    public int GetNewBeeSectionLength()
    {
        return (int)sectionLength.current;
    }
    public bool IsBeePresent()
	{
        //Debug.Log ("End: " + gameController.levelData.newSectionEnd + "  Top: " + gameController.levelData.levelTop);
        //if (gameController.levelData.newSectionEnd == gameController.levelData.levelTop+1)
        //{
        //	return true;
        //} else 
        //{
        //	return RandomLogger.RollBelowPercent(this,Utilities.RoundToNearestOrderOfMagnitude(difficultyPercent, 0.1f, 0.1f));

        //}
        return RandomLogger.RollBelowPercent(this, beeChance.current);
	}
}

