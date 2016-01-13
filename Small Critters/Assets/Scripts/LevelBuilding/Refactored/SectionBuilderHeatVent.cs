using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectionBuilderHeatVent : ISectionBuilder {

	
	public sectionBuilderType type {get;set;}
	LevelData levelData;
	GameObjectPoolManager poolManager;
	GameObject heatVent;
	IHeatVentSectionDifficulty difficultyManager;
	
	public SectionBuilderHeatVent(LevelData levelData, GameObjectPoolManager poolManager)
	{
		this.levelData = levelData;
		this.poolManager = poolManager;
		difficultyManager = ServiceLocator.getService<IHeatVentSectionDifficulty>();
		type = sectionBuilderType.heatVent;
		heatVent = Resources.Load("HeatVent") as GameObject;
		poolManager.addPool(heatVent, 20);
	}
	
	public void buildNewRow(List<GameObject> row)
	{
		if(!difficultyManager.IsHeatVentRowEmpty())
		{
			buildNewHeatVentRow(row);
		}
	}
	
	public void buildNewHeatVentRow(List<GameObject> row)
	{
		GameObject newVent = poolManager.retrieveObject("HeatVent");
		Vector3 newVentPosition = new Vector3(levelData.levelWidth * 0.5f, (float)levelData.levelTop + 1, 0f);
		Vector3 newVentRotation = difficultyManager.GetHeatVentRotation();
		newVent.transform.position = newVentPosition;
		newVent.transform.Rotate(newVentRotation);
		newVent.GetComponent<HeatVentController>().Configure(difficultyManager.GetHeatVentLength(), 
		                                                     difficultyManager.GetHeatVentFSMTimers(), 
		                                                     difficultyManager.GetHeatVentCycleOffset());
		
		row.Add (newVent);
	}

}
