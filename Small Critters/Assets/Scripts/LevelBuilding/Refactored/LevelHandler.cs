using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelHandler {

	public Queue<List<GameObject>> level;
	IGameData gameData;
	ISectionBuilderHandling sectionBuilderHandler;
	ISectionLenghtHandling sectionLenghtHandler;
	
	public LevelHandler(IGameData gameData, ISectionBuilderHandling sectionBuilderHandler, ISectionLenghtHandling sectionLenghtHandler)
	{
		this.gameData = gameData;
		
		level = new Queue<List<GameObject>>();
		for(int i =0; i< gameData.getLevelLeght(); ++i)
		{
			level.Enqueue(new List<GameObject>());
		}
		
		this.sectionBuilderHandler = sectionBuilderHandler;
		this.sectionLenghtHandler = sectionLenghtHandler;
	}
	
	public void buildNewRow() //TODO subscribe this to an event triggered by the fog
	{
		List<GameObject> row = level.Dequeue();
		level.Enqueue(sectionBuilderHandler.buildNewRow(row));
	}
}

public class SectionBuildersHandler: ISectionBuilderHandling
{
	public List<GameObject> buildNewRow(List<GameObject> row)
	{
		return row;
	}
}

public class SectionLenghtHandler
{
	
}
