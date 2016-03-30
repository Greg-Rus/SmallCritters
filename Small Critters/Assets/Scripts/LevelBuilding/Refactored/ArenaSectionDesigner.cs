using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaSectionDesigner : SectionDesigner, ISectionDesigning {

	private IArenaBuilding arenaBuilder;
	public ArenaSectionDesigner(ISectionBuilderSelection sectionBuilderSelector, LevelData levelData, IArenaBuilding arenaBuilder): base(sectionBuilderSelector,levelData)
	{
		this.arenaBuilder = arenaBuilder;
	}
	
	public new void buildNewRow(List<GameObject> row)
	{
		if(levelData.newSectionEnd == levelData.levelTop)
		{
			sectionBuilderSelector.selectNewSectionBuilder();
		}
		levelData.activeSectionBuilder.buildNewRow(row);
		arenaBuilder.SetUpArenaRow(row);
		levelData.levelTop+=1;
	}
}
