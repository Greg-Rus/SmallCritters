using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectionBuilderBees : ISectionBuilder {
	public SectionBuilderType type {get;set;}
	LevelData levelData;
	GameObjectPoolManager poolManager;
    ScoreHandler scoreHandler;
	IBeeSectionDifficulty difficultyManager;
	GameObject bee;
	List<GameObject> currentRow;
	
	
	public SectionBuilderBees(LevelData levelData, GameObjectPoolManager poolManager)
	{
		this.levelData = levelData;
		this.poolManager = poolManager;
		difficultyManager = ServiceLocator.getService<IBeeSectionDifficulty>();
        scoreHandler = ServiceLocator.getService<ScoreHandler>();
        type = SectionBuilderType.bees;
		bee = Resources.Load("Bee") as GameObject;
        BeeController beeController = bee.GetComponent<BeeController>();
        beeController.scoreHandler = scoreHandler;

        poolManager.addPool(bee, 20, 10);
		
	}
	
	public void buildNewRow(List<GameObject> row)
	{
		currentRow = row;
		buildNewBeeRow();
	}
	
	private void buildNewBeeRow()
	{
		DeployBeeAtPosition(1.5f); //Next to left wall
		DeployBeeAtPosition(levelData.navigableAreaWidth); //Next to rifht wall
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
			currentRow.Add (newBee);
		}
	}
	
	private void ConfigureBeeController(GameObject bee)
	{
		BeeController newBeeController = bee.GetComponent<BeeController>();
		newBeeController.Reset();
		newBeeController.chargeDistance = difficultyManager.GetChargeDistance();
		newBeeController.chargeSpeed = difficultyManager.GetChargeSpeed();
		newBeeController.chargeTime = difficultyManager.GetChargeTime();
		newBeeController.flySpeed = difficultyManager.GetFlySpeed();
	}

}
