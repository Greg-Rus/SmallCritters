using UnityEngine;
using System.Collections;

public class ProcessorSectionDifficultyManager : MonoBehaviour, IProcessorGroupDifficulty{
	
	public DifficultyManager mainDifficultyManager;
	
	public float difficultyPercent = 0f;
	public float difficultyPercentStep = 0.01f;

	public int[] processorPatternWeitghts = {5,4,3,2,1,0,0,0,0};
	public float[] processorPatternWeitghtsHistogram;
	
	public float processorPatternCyclesPerGroup = 1;
	
	public DifficultyParameter processorSectionLength;
	public DifficultyParameter processorStayCoolTime;
	public DifficultyParameter processorHeatUpTime;
	public DifficultyParameter processorStayHotTime;
	public DifficultyParameter processorCoolDownTime;
	
	private float processorPatternCycleOffset;
	private float nextPatternWeightStepThreshold;
	private int patternWeightsMaxValueIndex = 0;
	
	void Awake()
	{
		processorPatternWeitghtsHistogram = new float[processorPatternWeitghts.Length];
		CalculateProcessorPatternHistogram();
		nextPatternWeightStepThreshold = 1f / processorPatternWeitghts.Length;
	}

	public int GetNewProcessorSectionLenght()
	{
		return (int)UnityEngine.Random.Range(processorSectionLength.min, processorSectionLength.current);
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
		
		if(processorPatternWeitghtsHistogram == null)
		{
			processorPatternWeitghtsHistogram = new float[processorPatternWeitghts.Length];
		}
		processorPatternWeitghtsHistogram[0] = processorPatternWeitghts[0] * normalizedUnit;
		for (int i = 1; i < processorPatternWeitghtsHistogram.Length; ++i)
		{
			processorPatternWeitghtsHistogram[i] = processorPatternWeitghts[i] * normalizedUnit + processorPatternWeitghtsHistogram[i-1];
		}
	}
	public float GetProcessorPatternCycleOffset()
	{
		return (1f / mainDifficultyManager.levelData.navigableAreaWidth) * processorPatternCyclesPerGroup;
	}
	
	public float[] GetProcessorFSMTimers()
	{
		float[] timers = new float[]{processorStayCoolTime.current, processorHeatUpTime.current, processorStayHotTime.current, processorCoolDownTime.current};
		return timers;
	}
	
	public void ScaleDifficulty()
	{
		if(difficultyPercent< 1f)
		{
			ScaleProcessorPatternWeights();
			CalculateProcessorPatternHistogram();
			processorSectionLength.scaleCurrent(difficultyPercent);
			ScaleProcessorTimers();
		}
	}
	
	public void ScaleProcessorTimers()
	{
		difficultyPercent += difficultyPercentStep;
		processorStayCoolTime.scaleCurrent(difficultyPercent);
		processorHeatUpTime.scaleCurrent(difficultyPercent);
		processorStayHotTime.scaleCurrent(difficultyPercent);
		processorCoolDownTime.scaleCurrent(difficultyPercent);
	}
		
	private void ScaleProcessorPatternWeights()
	{
	
		if(difficultyPercent >= nextPatternWeightStepThreshold)
		{
			int maxWeight = processorPatternWeitghts[patternWeightsMaxValueIndex];
			++patternWeightsMaxValueIndex;
			nextPatternWeightStepThreshold = (1f / processorPatternWeitghts.Length) * (patternWeightsMaxValueIndex + 1);

			for(int i = patternWeightsMaxValueIndex ; i<processorPatternWeitghts.Length; ++i)
			{
				int newWeight = maxWeight - (i - patternWeightsMaxValueIndex);
				if(newWeight < 0) newWeight = 0;
				processorPatternWeitghts[i] = newWeight;
			}
			for(int i = patternWeightsMaxValueIndex -1 ; i >= 0; i--)
			{
				int newWeight = maxWeight - (patternWeightsMaxValueIndex - i);
				if(newWeight < 0) newWeight = 0;
				processorPatternWeitghts[i] = newWeight;
			}
		}
	}
}
