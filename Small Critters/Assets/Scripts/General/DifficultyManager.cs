using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class DifficultyManager: IBladeSectionLength, IProcessorSectionLenght, IProcessorGroupPattern {
	public int highestRowReached;
	public float bladeSpeedMin;
	public float bladeSpeedMax;
	public int bladeSectionLengthMin = 4;
	public int bladeSectionLengthMax = 10;
	public int bladeProcessorLengthMin = 4;
	public int bladeProcessorLengthMax = 7;
	private int[] processorPatternWeitghts;
	public int[] ProcessorPatternWeitghts //= {5,5,4,4,3,2,0,0,0};
	{
		get
		{
			return  processorPatternWeitghts;
		}
		set
		{
			processorPatternWeitghts = value;
			RecvalculateHistogram();
		}
	}
	public float[] processorPatternWeitghtsHistogram;
	
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
		return 1;
	}
	private void RecvalculateHistogram()
	{
		
	}

}
