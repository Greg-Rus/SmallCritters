using UnityEngine;
using System.Collections;

public class LevelRow {

	private GameObject[] hazards;
	private GameObject[] scenery;
	private GameObjectPoolManager pools;
	
	public LevelRow(GameObjectPoolManager pools)
	{
		this.pools = pools;
	}
	
	public void addHazard(GameObject[] newHazards)
	{
		hazards = newHazards;
	}
	
	public void addHazard(GameObject hazard)
	{
		hazards = new GameObject[1];
		hazards[0] = hazard;
	}
	
	public void addSceneryObjects(GameObject[] sceneryObjects)
	{
		scenery = sceneryObjects;
	}
	
	public void dismantle()
	{
		if(hazards != null)
		{
			foreach(GameObject item in hazards)
			{
				pools.storeObject(item);
			}
		}
		if(scenery != null)
		{
			foreach(GameObject item in scenery)
			{
				pools.storeObject(item);
			}
		}
	}
}
