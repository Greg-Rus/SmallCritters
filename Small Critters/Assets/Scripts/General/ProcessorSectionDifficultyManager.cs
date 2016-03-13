using UnityEngine;
using System.Collections;

public class ProcessorSectionDifficultyManager : MonoBehaviour, IProcessorGroupDifficulty{

	public float difficultyPercent = 0f;
	public float difficultyPercentStep = 0.01f;
	
	public DifficultyManager mainDifficultyManager;
//	public int processorSectionLengthMin = 4;
//	public int processorSectionLengthMax = 7;
	public DifficultyParameter processorSectionLength;
	
	public int[] processorPatternWeitghts = {5,4,3,2,1,0,0,0,0};
	public float[] processorPatternWeitghtsHistogram;
	
	public float processorPatternCyclesPerGroup = 1;
	
	public DifficultyParameter processorStayCoolTime;
	public DifficultyParameter processorHeatUpTime;
	public DifficultyParameter processorStayHotTime;
	public DifficultyParameter processorCoolDownTime;
//	public float processorStayCoolTime = 1f;
//	public float processorStayCoolTimeCap = 1f;
//	public float processorStayCoolTimeStep = 0.02f;
//	public float processorHeatUpTime = 1f;
//	public float processorHeatUpTimeCap = 0.5f;
//	public float processorHeatUpTimeStep = 0.02f;
//	public float processorStayHotTime = 1f;
//	public float processorStayHotTimeCap = 2f;
//	public float processorStayHotTimeStep = 0.04f;
//	public float processorCoolDownTime = 1f;
//	public float processorCoolDownTimeCap = 0.5f;
//	public float processorCoolDownTimeStep = 0.02f;
	
	private float processorPatternCycleOffset;
	public  float nextPatternWeightStepThreshold;
	public int patternWeightsMaxValueIndex = 0;
	
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
		ScaleProcessorPatternWeights(); //Do at every difficulty treshold?
		CalculateProcessorPatternHistogram();
		processorSectionLength.scaleCurrent(difficultyPercent);
		ScaleProcessorTimers();
	}
	
	public void ScaleProcessorTimers()
	{
		if(difficultyPercent< 1f)
		{
			difficultyPercent += difficultyPercentStep;
			processorStayCoolTime.scaleCurrent(difficultyPercent);
			processorHeatUpTime.scaleCurrent(difficultyPercent);
			processorStayHotTime.scaleCurrent(difficultyPercent);
			processorCoolDownTime.scaleCurrent(difficultyPercent);
		}		
	}
		
	private void ScaleProcessorPatternWeights()
	{
	
		if(difficultyPercent >= nextPatternWeightStepThreshold)
		{
			Debug.Log ("Scaling weights");
			int maxWeight = processorPatternWeitghts[patternWeightsMaxValueIndex];
			++patternWeightsMaxValueIndex;
			nextPatternWeightStepThreshold = (1f / processorPatternWeitghts.Length) * (patternWeightsMaxValueIndex + 1);
			Debug.Log ("Next At: " + nextPatternWeightStepThreshold + " = 1/" + processorPatternWeitghts.Length + " * " + patternWeightsMaxValueIndex + " + 1" );
			//processorPatternWeitghts[patternWeightsMaxValueIndex] = maxWeight;
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
//			int pivotPoint = 0;
//			for(int i = 0; i< processorPatternWeitghts.Length; ++i)
//			{
//				if(processorPatternWeitghts[i] == 5)
//				{
//					pivotPoint = i;
//					break;
//				}
//			}
//			bool rightAlligned = AlignWeightsOnRightFrom(pivotPoint);
//			bool leftAlligned = AlignWeightsOnLeftFrom(pivotPoint);
//			if(rightAlligned && leftAlligned)
//			{
//				processorPatternWeitghts[pivotPoint] -= 1;
//				processorPatternWeitghts[pivotPoint+1] += 1;
//			}
		}
		
	}
//	private bool AlignWeightsOnLeftFrom(int pivotPoint)
//	{
//		for(int i = pivotPoint; i > 0; --i)
//		{
//			if(processorPatternWeitghts[i-1] == 0 && processorPatternWeitghts[i] == 0)
//			{
//				continue;
//			}
//			if(processorPatternWeitghts[i-1] != processorPatternWeitghts[i]-1)
//			{
//				processorPatternWeitghts[i-1] -= 1;
//				if(processorPatternWeitghts[i-1] < 0)
//				{
//					processorPatternWeitghts[i-1] = 0;
//				}
//				return false;
//			}
//		}
//		return true;
//	}
	
	
//	private bool AlignWeightsOnRightFrom(int pivotPoint)
//	{
//		for(int i = pivotPoint; i < processorPatternWeitghts.Length -1; ++i)
//		{
//			if(processorPatternWeitghts[i+1] == 0 && processorPatternWeitghts[i] == 0)
//			{
//				continue;
//			}
//			if(processorPatternWeitghts[i+1] != processorPatternWeitghts[i]-1)
//			{
//				processorPatternWeitghts[i+1] += 1;
//				return false;
//			}
//			
//		}
//		return true;
//	}
	
	private bool IsMaxPatternWeightReached()
	{
		if(processorPatternWeitghts[processorPatternWeitghts.Length -1] == 5)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
