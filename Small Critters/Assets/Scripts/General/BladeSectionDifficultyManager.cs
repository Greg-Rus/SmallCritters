using UnityEngine;
using System.Collections;

public class BladeSectionDifficultyManager : MonoBehaviour , IBladeSectionDifficulty{

	public float difficultyPercent = 0f;
	public float difficultyPercentStep = 0.01f;
	public DifficultyParameter bladeSpeed;
	public DifficultyParameter bladeSectionLength;
	public DifficultyParameter bladeGap;
	public DifficultyParameter emptyRowChance;	
	
	public bool IsBladeRowEmpty()
	{
		return Utilities.RollBelowPercent(emptyRowChance.current);
	}
	public float GetBladeSpeed()
	{
		return UnityEngine.Random.Range(bladeSpeed.min, bladeSpeed.current);
	}
	public int GetNewBladeSectionLenght()
	{
		return (int)UnityEngine.Random.Range(bladeSectionLength.min, bladeSectionLength.current);
	}
	public float GetBladeGap()
	{
		return UnityEngine.Random.Range(bladeGap.min, bladeGap.current);
	}
	public float GetBladeRowCycleOffset()
	{
		return UnityEngine.Random.Range(0f,1f);
	}
	
	public void ScaleDifficulty()
	{
		if(difficultyPercent< 1f)
		{
			difficultyPercent += difficultyPercentStep;
			bladeSpeed.scaleCurrent(difficultyPercent);
			bladeSectionLength.scaleCurrent(difficultyPercent);
			bladeGap.scaleCurrent(difficultyPercent);
			emptyRowChance.scaleCurrent(difficultyPercent);
		}
	}
}
