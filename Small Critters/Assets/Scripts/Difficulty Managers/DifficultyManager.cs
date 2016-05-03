using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DifficultyManager: MonoBehaviour, IDifficultyBasedBuilderPicking{

	public BladeSectionDifficultyManager bladeSectionDifficultyManager;
	public ProcessorSectionDifficultyManager processorSectionDifficultyManager;
	public HeatVentDifficultyManager heatVentDifficultyManager;
	public BeeSectionDifficultyManager beeSectionDifficultyManager;
    public BugsDifficultyManager bugsDifficultyManager;
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
	
	public List<BuilderWeight> builderWeights;

	private int nextDifficultyScalingPoint;
	[NonSerialized]
	public LevelData levelData;
	public bool fogEnabled;
	
	public void Awake()
	{
		nextDifficultyScalingPoint = difficultyScalingThreshold;
		if(difficultyLevel < startingDifficultyLevel)
		{
			int steps = startingDifficultyLevel - difficultyLevel;
			for (int i = 0; i<steps;++i)
			{
				ScaleDifficulty();
			}
		}
	}
	public void HanldeNewHighestRowReached(object sender, NewRowReached newHighestRowReached)
	{
		highestRowReached = newHighestRowReached.newRowReached;
	}	
	
	public SectionBuilderType GetSectionBuilder()
	{
		float weightSum = 0;
		SectionBuilderType builder= SectionBuilderType.clear;
		for(int i = 0; i < builderWeights.Count; ++i)
		{
			weightSum+=builderWeights[i].weight;
		}
		float goal = RandomLogger.GetRandomRange (this,0,weightSum);
		float progress = 0;
		for(int i = 0; i < builderWeights.Count; ++i)
		{
			progress+=builderWeights[i].weight;
			if(progress>=goal)
			{
				builder = builderWeights[i].type;
				break;
			}
		}
		return builder;
	}
		
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
		bladeSectionDifficultyManager.ScaleDifficulty();
		processorSectionDifficultyManager.ScaleDifficulty();
		heatVentDifficultyManager.ScaleDifficulty();
		beeSectionDifficultyManager.ScaleDifficulty();
        bugsDifficultyManager.ScaleDifficulty();

    }

}

