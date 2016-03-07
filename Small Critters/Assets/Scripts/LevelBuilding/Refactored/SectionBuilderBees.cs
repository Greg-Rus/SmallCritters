using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectionBuilderBees : ISectionBuilder {
	public sectionBuilderType type {get;set;}
	LevelData levelData;
	GameObjectPoolManager poolManager;
	IBeeSectionDifficulty difficultyManager;
	GameObject bee;
	
	
	public SectionBuilderBees(LevelData levelData, GameObjectPoolManager poolManager)
	{
		this.levelData = levelData;
		this.poolManager = poolManager;
		difficultyManager = ServiceLocator.getService<IBeeSectionDifficulty>();
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
		DeployBeeAtPosition(1.5f); //Next to left wall
		DeployBeeAtPosition(levelData.navigableAreaWidth - 0.5f); //Next to rifht wall
	}
	
	private void DeployBeeAtPosition(float xCoordiante)
	{
		if(difficultyManager.IsBeePresent())
		{
			GameObject newBee = poolManager.retrieveObject("Bee");
			Vector3 newBeePosition = new Vector3(xCoordiante, levelData.levelTop, 0f);
			newBee.transform.position = newBeePosition;
			ConfigureBeeController(newBee);
			if(xCoordiante > levelData.levelWidth * 0.5f)
			{
				newBee.transform.Rotate(new Vector3(0f, 0f, 180f)); // Bee faces right by default
			}
		}
	}
	
	private void ConfigureBeeController(GameObject bee)
	{
		BeeController newBeeController = bee.GetComponent<BeeController>();
		newBeeController.chargeDistance = difficultyManager.GetChargeDistance();
		newBeeController.chargeSpeed = difficultyManager.GetChargeSpeed();
		newBeeController.chargeTime = difficultyManager.GetChargeTime();
		newBeeController.flySpeed = difficultyManager.GetFlySpeed();
	}

}
