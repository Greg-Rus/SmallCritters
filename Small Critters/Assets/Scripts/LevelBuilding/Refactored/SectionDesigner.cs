using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectionDesigner: ISectionDesigning
{
	
	protected ISectionBuilderSelection sectionBuilderSelector;
	protected LevelData levelData;
	public ISectionBuilder activeSectionBuilder;
	
	public SectionDesigner(ISectionBuilderSelection sectionBuilderSelector, LevelData levelData)
	{
		this.sectionBuilderSelector = sectionBuilderSelector;
		this.levelData = levelData;
		//sectionBuilderSelector.selectNewSectionBuilder();
	}
	
	public void buildNewRow(List<GameObject> row)
	{
		if(levelData.newSectionEnd == levelData.levelTop)
		{
			sectionBuilderSelector.selectNewSectionBuilder();
		}
		levelData.activeSectionBuilder.buildNewRow(row);
		levelData.levelTop+=1;
    }
}
