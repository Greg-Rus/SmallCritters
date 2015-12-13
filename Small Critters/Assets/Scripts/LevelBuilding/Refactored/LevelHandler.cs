using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelHandler {

	public Queue<List<GameObject>> level;
	LevelData levelData;
	ISectionDesigning sectionDesigner;
	
	public LevelHandler(LevelData levelData, ISectionDesigning sectionDesigner)
	{
		this.levelData = levelData;
		this.sectionDesigner = sectionDesigner;
		
		level = new Queue<List<GameObject>>();
		for(int i =0; i< levelData.levelLength; ++i)
		{
			level.Enqueue(new List<GameObject>());
		}
	}
	
	public void buildNewRow() //TODO subscribe this to an event triggered by the fog
	{
		List<GameObject> row = level.Dequeue();
		//TODO dismantleOldRow(row);
		sectionDesigner.buildNewRow(row);
		level.Enqueue(row);
	}
}

