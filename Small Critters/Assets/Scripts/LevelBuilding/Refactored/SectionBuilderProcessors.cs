using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectionBuilderProcessors : ISectionBuilder {

	public sectionBuilderType type {get;set;}
	LevelData levelData;
	GameObjectPoolManager poolManager;
	GameObject processor;
	
	public SectionBuilderProcessors(LevelData levelData, GameObjectPoolManager poolManager)
	{
		this.levelData = levelData;
		this.poolManager = poolManager;
		type = sectionBuilderType.processor;
		processor = Resources.Load("Processor") as GameObject;
		poolManager.addPool(processor, 200);
	}
	
	public void buildNewRow(List<GameObject> row)
	{
		if(levelData.levelTop+1 == levelData.newSectionEnd)
		{
			//activate the controller
		}

	}

}
