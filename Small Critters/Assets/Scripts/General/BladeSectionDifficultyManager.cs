using UnityEngine;
using System.Collections;

public class BladeSectionDifficultyManager : MonoBehaviour , IBladeSectionDifficulty{

	public DifficultyManager mainDifficultyManager;
	public float bladeSpeedMin = 0.5f; //scales up until Max value is reached.
	public float bladeSpeedMinCap = 2.8f;
	public float bladeSpeedMax = 3f;
	public float bladeSpeedScalingFactor = 0.02f; 
	
	public int bladeSectionLengthMin = 4;
	public int bladeSectionLengthMax = 10;
	
	public float bladeGapMin = 3;
	public float bladeGapMinCap = 2.5f;
	public float bladeGapMax = 5; //scales down until Min value is reached.
	public float bladeGapMaxCap = 3;
	public float bladeGapScalingFactor = -0.1f;
	
	public float chanceForAnEmptyRowInBladeSection = 0.5f;
	public float chanceForAnEmptyRowInBladeSectionCap = 0.25f;
	public float EmptyRowInBladeSectionScalingFactor = -0.01f; //scales to zero
	
	
	public bool IsBladeRowEmpty()
	{
		return Utilities.RollBelowPercent(chanceForAnEmptyRowInBladeSection);
	}
	public float GetBladeSpeed()
	{
		return UnityEngine.Random.Range(bladeSpeedMin,bladeSpeedMax);
	}
	public int GetNewBladeSectionLenght()
	{
		return UnityEngine.Random.Range(bladeSectionLengthMin, bladeSectionLengthMax);
	}
	public float GetBladeGap()
	{
		return UnityEngine.Random.Range(bladeGapMin, bladeGapMax);
	}
	public float GetBladeRowCycleOffset()
	{
		return UnityEngine.Random.Range(0f,1f);
	}
	
	public void ScaleDifficulty()
	{
		ScaleBladeSpeed();
		ScaleBladeEmptyRowChance();
		ScaleBladeGap();
	}
	
	private void ScaleBladeSpeed()
	{
		if(bladeSpeedMin + bladeSpeedScalingFactor < bladeSpeedMax)
		{
			bladeSpeedMin += bladeSpeedScalingFactor;
		}
	}
	
	private void ScaleBladeEmptyRowChance()
	{
		if(chanceForAnEmptyRowInBladeSection + EmptyRowInBladeSectionScalingFactor > chanceForAnEmptyRowInBladeSectionCap)
		{
			chanceForAnEmptyRowInBladeSection += EmptyRowInBladeSectionScalingFactor;
		}
	}
	
	private void ScaleBladeGap()
	{
		if(bladeGapMax + bladeGapScalingFactor > bladeGapMaxCap)
		{
			bladeGapMax += bladeGapScalingFactor;
		}
		
		if(bladeGapMin + bladeGapScalingFactor > bladeGapMinCap)
		{
			bladeGapMin += bladeGapScalingFactor;
		}
	}
}
