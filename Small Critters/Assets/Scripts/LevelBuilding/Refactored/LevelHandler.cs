using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelHandler {

	public Queue<List<GameObject>> level;
	ISectionDesigning sectionDesigner;
	IRowCleanup rowCleaner;
	
	public LevelHandler(LevelData levelData, ISectionDesigning sectionDesigner, IRowCleanup rowCleaner)
	{
		this.sectionDesigner = sectionDesigner;
		this.rowCleaner = rowCleaner;
		
		level = new Queue<List<GameObject>>();
		for(int i =0; i< levelData.levelLength; ++i)
		{
			level.Enqueue(new List<GameObject>());
		}
	}
	
	public void buildNewRow() //TODO subscribe this to an event triggered by the fog
	{
		List<GameObject> row = level.Dequeue();
		DismantleOldRow(row);
		sectionDesigner.buildNewRow(row);
		level.Enqueue(row);
	}
	
	private void DismantleOldRow(List<GameObject> row)
	{
		rowCleaner.DismantleRow(row);
	}
}

