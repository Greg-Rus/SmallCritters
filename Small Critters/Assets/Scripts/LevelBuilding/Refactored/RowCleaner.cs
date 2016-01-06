using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RowCleaner : IRowCleanup {

	GameObjectPoolManager poolManager;
	
	public RowCleaner(GameObjectPoolManager poolManager)
	{
		this.poolManager = poolManager;
	}
	
	public void DismantleRow(List<GameObject> row)
	{
		if(row.Count != 0)
		{
			foreach(GameObject gameObject in row)
			{
				poolManager.storeObject(gameObject);
			}
		}
		row.Clear();
	}
}
