using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelHandler {

	public Queue<List<GameObject>> level;
	IGameData gameData;
	ISectionDesigning sectionDesigner;
	
	public LevelHandler(IGameData gameData, ISectionDesigning sectionDesigner)
	{
		this.gameData = gameData;
		this.sectionDesigner = sectionDesigner;
		
		level = new Queue<List<GameObject>>();
		for(int i =0; i< gameData.getLevelLeght(); ++i)
		{
			level.Enqueue(new List<GameObject>());
		}
	}
	
	public void buildNewRow() //TODO subscribe this to an event triggered by the fog
	{
		List<GameObject> row = level.Dequeue();
		level.Enqueue(sectionDesigner.buildNewRow(row));
	}
}

