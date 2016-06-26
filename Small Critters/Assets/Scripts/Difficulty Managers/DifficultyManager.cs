using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DifficultyManager: MonoBehaviour, IDifficultyBasedBuilderPicking{

	public BladeSectionDifficultyManager bladeSectionDifficultyManager;
	public ProcessorSectionDifficultyManager processorSectionDifficultyManager;
	public HeatVentDifficultyManager heatVentDifficultyManager;
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
    private SectionBuilderType bannedSection;
    private SectionBuilderType nextBannedSection;
    private float bannedSectionWeight;
	
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
        bannedSection = builderWeights[0].type;
        bannedSectionWeight = builderWeights[0].weight;
        nextBannedSection = SectionBuilderType.bugs;

    }
	public void HanldeNewHighestRowReached(object sender, NewRowReached newHighestRowReached)
	{
		highestRowReached = newHighestRowReached.newRowReached;
	}	
	
	public SectionBuilderType GetSectionBuilder()
	{
		float weightSum = 0;
        CycleBannedSection();
        SectionBuilderType builder = SectionBuilderType.clear;
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

    public void BanSectionType(SectionBuilderType sectionTypeToBan)
    {
        nextBannedSection = sectionTypeToBan;
    }

    private void CycleBannedSection()
    {
        UnbanSection();
        BanNextSection(nextBannedSection);
    }

    private void UnbanSection()
    {
        GetBuilderWeightByType(bannedSection).weight = bannedSectionWeight;
    }
    private void StoreBannedSectionInfo(BuilderWeight builderWeight)
    {
        bannedSection = builderWeight.type;
        bannedSectionWeight = builderWeight.weight;
    }
    private void BanNextSection(SectionBuilderType sectionTypeToBan)
    {
        BuilderWeight newSectionToBan = GetBuilderWeightByType(sectionTypeToBan);
        StoreBannedSectionInfo(newSectionToBan);
        newSectionToBan.weight = 0f;
    }

    private BuilderWeight GetBuilderWeightByType(SectionBuilderType type)
    {
        BuilderWeight builderWeight = null;
        for (int i = 0; i < builderWeights.Count; ++i)
        {
            if (builderWeights[i].type == type)
            {
                builderWeight = builderWeights[i];
            }
        }
        return builderWeight;
    }

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
        bugsDifficultyManager.ScaleDifficulty();
    }
}

