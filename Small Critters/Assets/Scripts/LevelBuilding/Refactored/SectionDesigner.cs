using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectionDesigner: ISectionDesigning
{
	
	ISectionBuilderSelection sectionBuilderSelector;
	//ISectionBuilderConfiguration sectionBuilderConfigurator;
	LevelData levelData;
	public ISectionBuilder activeSectionBuilder;

	public int levelTop = 0;
	
	public SectionDesigner(ISectionBuilderSelection sectionBuilderSelector/*, ISectionBuilderConfiguration sectionBuilderConfigurator*/, LevelData levelData)
	{
		this.sectionBuilderSelector = sectionBuilderSelector;
		//this.sectionBuilderConfigurator = sectionBuilderConfigurator;
		this.levelData = levelData;
		levelData.activeSectionBuilder = sectionBuilderSelector.selectNewSectionBuilder();
	}
	
	public List<GameObject> buildNewRow(List<GameObject> row)
	{
		if(levelData.activeSectionBuilder.toRow == levelTop)
		{
			levelData.activeSectionBuilder = sectionBuilderSelector.selectNewSectionBuilder();
		}
		levelData.levelTop+=1;
		return levelData.activeSectionBuilder.buildNewRow(row);
	}
}
