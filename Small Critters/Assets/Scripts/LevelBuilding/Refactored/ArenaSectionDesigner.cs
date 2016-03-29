using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaSectionDesigner : SectionDesigner, ISectionDesigning {

	private IArenaBuilding arenaBuilder;
	public ArenaSectionDesigner(ISectionBuilderSelection sectionBuilderSelector, LevelData levelData, IArenaBuilding arenaBuilder): base(sectionBuilderSelector,levelData)
	{
		this.arenaBuilder = arenaBuilder;
	}
	
	public void buildNewRow(List<GameObject> row)
	{
		arenaBuilder.SetUpArenaRow(row);
		base.buildNewRow(row);
	}
}
