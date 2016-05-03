using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SectionBuilderSelector: ISectionBuilderSelection {

	public Dictionary<SectionBuilderType, ISectionBuilder> availableSectionBuilders;
	private ISectionBuilderConfiguration sectionBuilderConfigurator;
	private LevelData levelData;
	private IDifficultyBasedBuilderPicking difficultyManager;
	
	
	public SectionBuilderSelector (ISectionBuilderConfiguration sectionBuilderConfigurator, LevelData levelData)
	{
		this.sectionBuilderConfigurator = sectionBuilderConfigurator;
		this.levelData = levelData;
		difficultyManager = ServiceLocator.getService<IDifficultyBasedBuilderPicking>();
		availableSectionBuilders = new Dictionary<SectionBuilderType, ISectionBuilder>();
	}
	
	public void addSectionBuilder (ISectionBuilder sectionBuilder)
	{
		availableSectionBuilders.Add(sectionBuilder.type,sectionBuilder);
	}
	
	public void selectNewSectionBuilder()
	{
		SectionBuilderType newBuilderType;
		if(levelData.activeSectionBuilder.type != SectionBuilderType.clear)
		{
			newBuilderType = SectionBuilderType.clear;
		}
		else
		{
			newBuilderType = difficultyManager.GetSectionBuilder();//Random.Range(1, availableSectionBuilders.Count); //TODO candidate for DefficultyManager
			//Debug.Log (newBuilderType);
		}
		
		levelData.activeSectionBuilder = availableSectionBuilders[newBuilderType];//[newBuilderType];
		sectionBuilderConfigurator.configureSectionBuilder();
	}

}
