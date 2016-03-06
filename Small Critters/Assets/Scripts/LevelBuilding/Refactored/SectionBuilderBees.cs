using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectionBuilderBees : ISectionBuilder {
	public sectionBuilderType type {get;set;}
	LevelData levelData;
	GameObjectPoolManager poolManager;
	IBladeSectionDifficulty difficultyManager;
	GameObject bee;
	
	
	public SectionBuilderBees(LevelData levelData, GameObjectPoolManager poolManager)
	{
		this.levelData = levelData;
		this.poolManager = poolManager;
		difficultyManager = ServiceLocator.getService<IBladeSectionDifficulty>();
		type = sectionBuilderType.bees;
		bee = Resources.Load("Bee") as GameObject;
		
		poolManager.addPool(bee, 10);
		
	}
	
	public void buildNewRow(List<GameObject> row)
	{
			buildNewBeeRow(row);
	}
	
	private void buildNewBeeRow(List<GameObject> row)
	{
		
	}

}
