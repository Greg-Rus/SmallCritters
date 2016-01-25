using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SectionBuilderSelector: ISectionBuilderSelection {

	public Dictionary<sectionBuilderType, ISectionBuilder> availableSectionBuilders;
	private ISectionBuilderConfiguration sectionBuilderConfigurator;
	private LevelData levelData;
	private IDifficultyBasedBuilderPicking difficultyManager;
	
	
	public SectionBuilderSelector (ISectionBuilderConfiguration sectionBuilderConfigurator, LevelData levelData)
	{
		this.sectionBuilderConfigurator = sectionBuilderConfigurator;
		this.levelData = levelData;
		difficultyManager = ServiceLocator.getService<IDifficultyBasedBuilderPicking>();
		availableSectionBuilders = new Dictionary<sectionBuilderType, ISectionBuilder>();
	}
	
	public void addSectionBuilder (ISectionBuilder sectionBuilder)
	{
		availableSectionBuilders.Add(sectionBuilder.type,sectionBuilder);
	}
	
	public void selectNewSectionBuilder()
	{
		sectionBuilderType newBuilderType;
		if(levelData.activeSectionBuilder.type != sectionBuilderType.clear)
		{
			newBuilderType = sectionBuilderType.clear;
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
