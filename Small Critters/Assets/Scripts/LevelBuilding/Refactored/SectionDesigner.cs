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
	
	public SectionDesigner(ISectionBuilderSelection sectionBuilderSelector, LevelData levelData)
	{
		this.sectionBuilderSelector = sectionBuilderSelector;
		this.levelData = levelData;
		sectionBuilderSelector.selectNewSectionBuilder();
	}
	
	public List<GameObject> buildNewRow(List<GameObject> row)
	{
		if(levelData.newSectionEnd == levelTop)
		{
			sectionBuilderSelector.selectNewSectionBuilder();
		}
		levelData.levelTop+=1;
		return levelData.activeSectionBuilder.buildNewRow(row);
	}
}
