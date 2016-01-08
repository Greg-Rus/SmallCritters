using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DifficultyManager: MonoBehaviour, IBladeSectionDifficulty, IProcessorGroupDifficulty {
	//TODO difficulty progression
	private int highestRowReached = 0;
	public int HighestRowReached
	{
		get{ return highestRowReached;}
		set
		{ 
		highestRowReached = value;
		ScaleDifficulty();
		}
	}
	public int difficultyScalingThreshold = 50;
	private int nextDifficultySclingPoint;
	public float bladeSpeedMin = 0.5f; //scales up until Max value is reached.
	public float bladeSpeedMax = 3f; //TODO if max value is reached similar rows will apear in sequence. Offset the cycle or make random return varied results
	public float bladeSpeedScalingFactor = 0.2f; 
	
	public int bladeSectionLengthMin = 4;
	public int bladeSectionLengthMax = 10;
	
	public float bladeGapMin = 3;
	public float bladeGapMax = 5; //scales down until Min value is reached.
	public float bladeGapScalingFactor = -0.2f;
	
	public float chanceForAnEmptyRowInBladeSection = 0.25f;
	public float EmptyRowInBladeSectionScalingFactor = -0.01f; //scales to zero
	
	public int processorSectionLengthMin = 4;
	public int processorSectionLengthMax = 7;
	
	public int[] processorPatternWeitghts = {5,4,3,2,1,0,0,0,0};
	public float[] processorPatternWeitghtsHistogram;
	
	public float processorPatternCyclesPerGroup = 1;
	
	public float stayCoolTime = 1;
	public float heatUpTime = 1;
	public float stayHotTime = 1;
	public float coolDownTime = 1;
	
	private float processorPatternCycleOffset;
	[NonSerialized]
	public LevelData levelData;
	
	public void Start()
	{
		processorPatternWeitghtsHistogram = new float[processorPatternWeitghts.Length];
		CalculateProcessorPatternHistogram();
		nextDifficultySclingPoint = difficultyScalingThreshold;
	}
	
	public float GetBladeSpeed()
	{
		return UnityEngine.Random.Range(bladeSpeedMin,bladeSpeedMax);
	}
	
	public void HanldeNewHighestRowReached(object sender, NewRowReached newHighestRowReached)
	{
		highestRowReached = newHighestRowReached.newRowReached;
	}	
	
	public int GetNewBladeSectionLenght()
	{
		return UnityEngine.Random.Range(bladeSectionLengthMin, bladeSectionLengthMax);
	}
	
	public int GetNewProcessorSectionLenght()
	{
		return UnityEngine.Random.Range(processorSectionLengthMin, processorSectionLengthMax);
	}
	
	public int GetNewProcessorGroupPattern()
	{
		float randomPercent = UnityEngine.Random.Range (0f,1f);
		int bucketNumber = 0;
			for( ; bucketNumber <  processorPatternWeitghtsHistogram.Length ; ++bucketNumber)
		{
			if(randomPercent <= processorPatternWeitghtsHistogram[bucketNumber])
			{
				break;
			}
		}
		return bucketNumber;
	}
	private void CalculateProcessorPatternHistogram()
	{
		int weightSum = 0;
		foreach(int weight in processorPatternWeitghts)
		{
			weightSum += weight;
		}
		
		float normalizedUnit = 1f / weightSum;
		
		processorPatternWeitghtsHistogram[0] = processorPatternWeitghts[0] * normalizedUnit;
		for (int i = 1; i < processorPatternWeitghtsHistogram.Length; ++i)
		{
			processorPatternWeitghtsHistogram[i] = processorPatternWeitghts[i] * normalizedUnit + processorPatternWeitghtsHistogram[i-1];
		}
	}
	public float GetProcessorPatternCycleOffset()
	{
		return (1f / levelData.navigableAreaWidth) * processorPatternCyclesPerGroup;
	}
	
	public float[] GetProcessorFSMTimers()
	{
		float[] timers = new float[]{stayCoolTime, heatUpTime, stayHotTime, coolDownTime};
		return timers;
	}
	public float GetBladeGap()
	{
		return UnityEngine.Random.Range(bladeGapMin, bladeGapMax);
	}
	public bool IsEmptyRow()
	{
		float roll = UnityEngine.Random.Range (0f,1f);
		if(roll <= chanceForAnEmptyRowInBladeSection)
		{
			return true;
		}
		else
		{
			return false;
		}
		
	}
	public void ScaleDifficulty()
	{
		if(highestRowReached >= nextDifficultySclingPoint)
		{
			nextDifficultySclingPoint += difficultyScalingThreshold;
			ScaleBladeSectionDifficulty();
			SclaeProcessorSectionDifficulty();
		}
	}
	private void ScaleBladeSectionDifficulty()
	{

		if(bladeSpeedMin + bladeSpeedScalingFactor < bladeSpeedMax)
		{
			bladeSpeedMin += bladeSpeedScalingFactor;
		}
		else
		{
			bladeSpeedMin = bladeSpeedMax;
		}
		
		if(chanceForAnEmptyRowInBladeSection + EmptyRowInBladeSectionScalingFactor > 0f)
		{
			chanceForAnEmptyRowInBladeSection += EmptyRowInBladeSectionScalingFactor;
		}
		else
		{
			chanceForAnEmptyRowInBladeSection = 0f;
		}
		
		if(bladeGapMax + bladeGapScalingFactor > bladeGapMin)
		{
			bladeGapMax += bladeGapScalingFactor;
		}
		else
		{
			bladeGapMax = bladeGapMin;
		}
	}
	private void SclaeProcessorSectionDifficulty()
	{
	
	}

}
