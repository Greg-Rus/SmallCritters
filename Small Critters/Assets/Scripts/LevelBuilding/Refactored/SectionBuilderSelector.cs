using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SectionBuilderSelector: ISectionBuilderSelection {

	public List<ISectionBuilder> availableSectionBuilders;
	private ISectionBuilderConfiguration sectionBuilderConfigurator;
	private LevelData levelData;
	
	public SectionBuilderSelector (ISectionBuilderConfiguration sectionBuilderConfigurator, LevelData levelData)
	{
		this.sectionBuilderConfigurator = sectionBuilderConfigurator;
		this.levelData = levelData;
		availableSectionBuilders = new List<ISectionBuilder>();
	}
	
	public void addSectionBuilder (ISectionBuilder sectionBuilder)
	{
		availableSectionBuilders.Add(sectionBuilder);
	}
	
	public void selectNewSectionBuilder()
	{
		int newBuilder;
		if(levelData.activeSectionBuilder.type != sectionBuilderType.clear)
		{
			newBuilder = 0;
		}
		else
		{
			newBuilder = Random.Range(1, availableSectionBuilders.Count);
		}
		levelData.activeSectionBuilder = availableSectionBuilders[newBuilder];//[newBuilderType];
		sectionBuilderConfigurator.configureSectionBuilder();
	}

}
