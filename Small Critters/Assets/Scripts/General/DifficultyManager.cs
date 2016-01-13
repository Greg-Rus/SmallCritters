using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DifficultyManager: MonoBehaviour, IBladeSectionDifficulty, IProcessorGroupDifficulty, IHeatVentSectionDifficulty {
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
	
	public float processorStayCoolTime = 1;
	public float processorHeatUpTime = 1;
	public float processorStayHotTime = 1;
	public float processorCoolDownTime = 1;
	
	private float processorPatternCycleOffset;
	
	public int heatVentSectionLengthMin = 5;
	public int heatVentSectionLengthMax = 10;
	public float minHeatVentLenght = 2f;
	public float maxHeatVentLenght = 7f;
	public float chanceForEmptyRowInHeatVentSection = 0.5f;
	public float heatVentClosedTime = 1f;
	public float heatVentOpeningTime = 0.4f;
	public float heatVentWarmingUpTime = 1f;
	public float heatVentVentingTime = 1.5f;
	public float heatVentClosingTime = 1f;

	
	[NonSerialized]
	public LevelData levelData;
	
	public bool IsHeatVentRowEmpty()
	{
		return RollBelowPercent(chanceForEmptyRowInHeatVentSection);
	}
	public bool IsBladeRowEmpty()
	{
		return RollBelowPercent(chanceForAnEmptyRowInBladeSection);
	}
	
	private float TimerModifier(float min, float max)
	{
		return UnityEngine.Random.Range(min,max);
	}
	
	public float[] GetHeatVentFSMTimers()
	{
		
		float[] timers = new float[]{heatVentClosedTime + TimerModifier(-0.5f, 2f), 
									heatVentOpeningTime, 
									heatVentWarmingUpTime + TimerModifier(-0.5f, 2f), 
									heatVentVentingTime + TimerModifier(-1f, 1.5f), 
									heatVentClosingTime};
		return timers;
	}
	
	public float GetHeatVentLength()
	{
		return UnityEngine.Random.Range(minHeatVentLenght,maxHeatVentLenght);
	}
	
	public float GetHeatVentCycleOffset()
	{
		return UnityEngine.Random.Range(0f,1f);
	}
	
	public Vector3 GetHeatVentRotation()
	{
		if(RollBelowPercent(0.5f))
		{return Vector3.zero;}
		else
		{return new Vector3(0f,0f,180f);}
	}
	
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
	
	public int GetNewHeatVentSectionLenght()
	{
		return UnityEngine.Random.Range(heatVentSectionLengthMin, heatVentSectionLengthMax);
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
		float[] timers = new float[]{processorStayCoolTime, processorHeatUpTime, processorStayHotTime, processorCoolDownTime};
		return timers;
	}
	public float GetBladeGap()
	{
		return UnityEngine.Random.Range(bladeGapMin, bladeGapMax);
	}
	public bool RollBelowPercent(float pecent)
	{
		float roll = UnityEngine.Random.Range (0f,1f);
		if(roll <= pecent)
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
			ScaleProcessorSectionDifficulty();
			ScaleHeatVentSectionDifficulty();
			
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
	private void ScaleProcessorSectionDifficulty()
	{
	
	}
	private void ScaleHeatVentSectionDifficulty()
	{
		
	}

}
