using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DifficultyManager: MonoBehaviour, IBladeSectionLength, IProcessorSectionLenght, IProcessorGroupPatternDifficulty {
	public int highestRowReached;
	public float bladeSpeedMin = 0.5f;
	public float bladeSpeedMax = 3f;
	public int bladeSectionLengthMin = 4;
	public int bladeSectionLengthMax = 10;
	public int bladeProcessorLengthMin = 4;
	public int bladeProcessorLengthMax = 7;
	public int[] processorPatternWeitghts = {5,5,4,4,3,2,0,0,0};
	public float[] processorPatternWeitghtsHistogram;
	public float processorPatternCyclesPerGroup = 1;
	public float stayCoolTime = 1;
	public float heatUpTime = 1;
	public float stayHotTime = 1;
	public float coolDownTime = 1;
	private float processorPatternCycleOffset;
	//private IProcessorFSM processorFSM;
	//[NonSerialized]
	public LevelData levelData;
	
	public void Start()
	{
		processorPatternWeitghtsHistogram = new float[processorPatternWeitghts.Length];
		CalculateProcessorPatternHistogram();
	}
	
	public float GetBladeSpeed()
	{
		return bladeSpeedMin;
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
		return UnityEngine.Random.Range(bladeProcessorLengthMin, bladeProcessorLengthMax);
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

}
