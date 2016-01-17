using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DifficultyManager: MonoBehaviour, IBladeSectionDifficulty, IProcessorGroupDifficulty, IHeatVentSectionDifficulty , IDifficultyBasedBuilderPicking{
	//TODO difficulty progression
	public BladeSectionDifficultyManager baldeSectionDifficultyManager;
	public ProcessorSectionDifficultyManager processorSectionDifficultyManager;
	public HeatVentDifficultyManager heatVentDifficultyManager;
	private int highestRowReached = 0;
	public int HighestRowReached
	{
		get{ return highestRowReached;}
		set
		{ 
			highestRowReached = value;
			CheckDifficultyThreshold();
		}
	}
	public int difficultyScalingThreshold = 50;
	public int difficultyLevel = 1;
	public int startingDifficultyLevel = 1;
	
	//public BuilderWeight[] builderWeights;
	public List<BuilderWeight> builderWeights;
	public float useBladeBuilder = 1f;
	public float useProcessorBuilder = 1f;
	public float useHeatVentBuilder = 1f;
	
	private int nextDifficultyScalingPoint;
	[NonSerialized]
	public LevelData levelData;
	
	public void Awake()
	{
		//builderWeights = new BuilderWeight[]{new BuilderWeight(sectionBuilderType.blade, 1f),
		//	new BuilderWeight(sectionBuilderType.processor, 1f),
		//	new BuilderWeight(sectionBuilderType.heatVent, 1f)
		//};
		nextDifficultyScalingPoint = difficultyScalingThreshold;
		if(difficultyLevel < startingDifficultyLevel)
		{
			int steps = startingDifficultyLevel - difficultyLevel;
			for (int i = 0; i<steps;++i)
			{
				ScaleDifficulty();
				Debug.Log (difficultyLevel);
			}
		}
	}
	public void HanldeNewHighestRowReached(object sender, NewRowReached newHighestRowReached)
	{
		highestRowReached = newHighestRowReached.newRowReached;
	}	
	
	public sectionBuilderType GetSectionBuilder()
	{
		float weightSum = 0;
		sectionBuilderType builder= sectionBuilderType.clear;
		for(int i = 0; i < builderWeights.Count; ++i)
		{
			weightSum+=builderWeights[i].weight;
		}
		float goal = UnityEngine.Random.Range (0,weightSum);
		Debug.Log ("Goal: " + goal);
		float progress = 0;
		for(int i = 0; i < builderWeights.Count; ++i)
		{
			progress+=builderWeights[i].weight;
			if(progress>=goal)
			{
				builder = builderWeights[i].type;
				Debug.Log ("GoalReached at: " + progress + " selecting: " + builder);
				break;
			}
		}
		return builder;
	}
	
	
	//IBladeSectionDifficulty
	public bool IsBladeRowEmpty(){return baldeSectionDifficultyManager.IsBladeRowEmpty();}
	public float GetBladeSpeed(){return baldeSectionDifficultyManager.GetBladeSpeed();}
	public int GetNewBladeSectionLenght(){return baldeSectionDifficultyManager.GetNewBladeSectionLenght();}
	public float GetBladeGap(){return baldeSectionDifficultyManager.GetBladeGap();}	
	public float GetBladeRowCycleOffset(){return baldeSectionDifficultyManager.GetBladeRowCycleOffset();}
	
	//IProcessorGroupDifficulty
	public int GetNewProcessorSectionLenght(){return processorSectionDifficultyManager.GetNewProcessorSectionLenght();}
	public int GetNewProcessorGroupPattern(){return processorSectionDifficultyManager.GetNewProcessorGroupPattern();}
	public float GetProcessorPatternCycleOffset(){return processorSectionDifficultyManager.GetProcessorPatternCycleOffset();}
	public float[] GetProcessorFSMTimers(){return processorSectionDifficultyManager.GetProcessorFSMTimers();}

	//IHeatVentSectionDifficulty
	public bool IsHeatVentRowEmpty(){return heatVentDifficultyManager.IsHeatVentRowEmpty();}
	public int GetNewHeatVentSectionLenght(){return heatVentDifficultyManager.GetNewHeatVentSectionLenght();}
	public float[] GetHeatVentFSMTimers(){return heatVentDifficultyManager.GetHeatVentFSMTimers();}
	public float GetHeatVentLength(){return heatVentDifficultyManager.GetHeatVentLength();}
	public float GetHeatVentCycleOffset(){return heatVentDifficultyManager.GetHeatVentCycleOffset();}
	public Vector3 GetHeatVentRotation(){return heatVentDifficultyManager.GetHeatVentRotation();}
	
	
	//Difficulty Scaling
	public void CheckDifficultyThreshold()
	{
		if(highestRowReached >= nextDifficultyScalingPoint)
		{
			nextDifficultyScalingPoint += difficultyScalingThreshold;
			ScaleDifficulty();
		}
	}
	
	public void ScaleDifficulty()
	{
		++difficultyLevel;
		baldeSectionDifficultyManager.ScaleDifficulty();
		processorSectionDifficultyManager.ScaleDifficulty();
		heatVentDifficultyManager.ScaleDifficulty();
	}

}

