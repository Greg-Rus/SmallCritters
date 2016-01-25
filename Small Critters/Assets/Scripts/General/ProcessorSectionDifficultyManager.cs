using UnityEngine;
using System.Collections;

public class ProcessorSectionDifficultyManager : MonoBehaviour, IProcessorGroupDifficulty{

	public DifficultyManager mainDifficultyManager;
	public int processorSectionLengthMin = 4;
	public int processorSectionLengthMax = 7;
	
	public int[] processorPatternWeitghts = {5,4,3,2,1,0,0,0,0};
	public float[] processorPatternWeitghtsHistogram;
	
	public float processorPatternCyclesPerGroup = 1;
	
	public float processorStayCoolTime = 1f;
	public float processorStayCoolTimeCap = 1f;
	public float processorStayCoolTimeStep = 0.02f;
	public float processorHeatUpTime = 1f;
	public float processorHeatUpTimeCap = 0.5f;
	public float processorHeatUpTimeStep = 0.02f;
	public float processorStayHotTime = 1f;
	public float processorStayHotTimeCap = 2f;
	public float processorStayHotTimeStep = 0.04f;
	public float processorCoolDownTime = 1f;
	public float processorCoolDownTimeCap = 0.5f;
	public float processorCoolDownTimeStep = 0.02f;
	
	private float processorPatternCycleOffset;
	
	void Awake()
	{
		processorPatternWeitghtsHistogram = new float[processorPatternWeitghts.Length];
		CalculateProcessorPatternHistogram();
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
		float[] timers = new float[]{processorStayCoolTime, processorHeatUpTime, processorStayHotTime, processorCoolDownTime};
		return timers;
	}
	
	public void ScaleDifficulty()
	{
		ScaleProcessorPatternWeights(); //Do at every difficulty treshold?
		CalculateProcessorPatternHistogram();
		//ScaleProcessorSectionLenght();
		ScaleProcessorTimers();
	}
	
	public void ScaleProcessorTimers()
	{
		if(processorStayCoolTime> processorStayCoolTimeCap)
		{
			processorStayCoolTime = processorStayCoolTime - processorStayCoolTimeStep;
		}
		if(processorHeatUpTime > processorHeatUpTimeCap)
		{
			processorHeatUpTime = processorHeatUpTime - processorHeatUpTimeStep;
		}
		if(processorStayHotTime < processorStayHotTimeCap)
		{
			processorStayHotTime = processorStayHotTime + processorStayHotTimeStep;
		}
		if(processorCoolDownTime > processorCoolDownTimeCap)
		{
			processorCoolDownTime = processorCoolDownTime - processorCoolDownTimeStep;
		}		
	}
	
	private void ScaleProcessorSectionLenght()
	{
		if(processorSectionLengthMin + processorSectionLengthMax % 2 == 0)
		{
			++processorSectionLengthMin;
		}
		else
		{
			++processorSectionLengthMax;
		}
	}
	
	private void ScaleProcessorPatternWeights()
	{
		if(!IsMaxPatternWeightReached())
		{
			int pivotPoint = 0;
			for(int i = 0; i< processorPatternWeitghts.Length; ++i)
			{
				if(processorPatternWeitghts[i] == 5)
				{
					pivotPoint = i;
					break;
				}
			}
			bool rightAlligned = AlignWeightsOnRightFrom(pivotPoint);
			bool leftAlligned = AlignWeightsOnLeftFrom(pivotPoint);
			if(rightAlligned && leftAlligned)
			{
				processorPatternWeitghts[pivotPoint] -= 1;
				processorPatternWeitghts[pivotPoint+1] += 1;
			}
		}
		
	}
	private bool AlignWeightsOnLeftFrom(int pivotPoint)
	{
		for(int i = pivotPoint; i > 0; --i)
		{
			if(processorPatternWeitghts[i-1] == 0 && processorPatternWeitghts[i] == 0)
			{
				continue;
			}
			if(processorPatternWeitghts[i-1] != processorPatternWeitghts[i]-1)
			{
				processorPatternWeitghts[i-1] -= 1;
				if(processorPatternWeitghts[i-1] < 0)
				{
					processorPatternWeitghts[i-1] = 0;
				}
				return false;
			}
		}
		return true;
	}
	
	
	private bool AlignWeightsOnRightFrom(int pivotPoint)
	{
		for(int i = pivotPoint; i < processorPatternWeitghts.Length -1; ++i)
		{
			if(processorPatternWeitghts[i+1] == 0 && processorPatternWeitghts[i] == 0)
			{
				continue;
			}
			if(processorPatternWeitghts[i+1] != processorPatternWeitghts[i]-1)
			{
				processorPatternWeitghts[i+1] += 1;
				return false;
			}
			
		}
		return true;
	}
	
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
