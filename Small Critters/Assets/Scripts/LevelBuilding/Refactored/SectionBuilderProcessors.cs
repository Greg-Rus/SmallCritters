using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectionBuilderProcessors : ISectionBuilder {

	public sectionBuilderType type {get;set;}
	LevelData levelData;
	GameObjectPoolManager poolManager;
	GameObject processor;
	ProcessorManager [,] processorGroup;
	GameObject newProcessorGroupController;
	
	
	public SectionBuilderProcessors(LevelData levelData, GameObjectPoolManager poolManager)
	{
		this.levelData = levelData;
		this.poolManager = poolManager;
		type = sectionBuilderType.processor;
		GameObject processor = Resources.Load("Processor") as GameObject;
		GameObject processorGroup = Resources.Load("ProcessorGroup") as GameObject;
		processorGroup.GetComponent<ProcessorGroupController> ().processorStateMachine = new ProcessorFSM (); //strong coupupling here. Used some DI container.
		poolManager.addPool(processor, 200);
		poolManager.addPool(processorGroup, 20);
	}
	
	public void buildNewRow(List<GameObject> row)
	{
		if(levelData.levelTop+1 == levelData.newSectionStart)
		{
			processorGroup = new ProcessorManager[levelData.navigableAreaWidth, levelData.newSectionEnd - levelData.newSectionStart +1];
		}
		
		for (int i =0; i < levelData.navigableAreaWidth; ++i)
		{
			GameObject newProcessor = poolManager.retrieveObject("Processor");
			processorGroup[i, levelData.levelTop+1 - levelData.newSectionStart] = newProcessor.GetComponent<ProcessorManager>();
			Vector3 newProcessorPosition;
			newProcessorPosition = new Vector3(1.5f+i,(float)levelData.levelTop + 1, 0f);
			newProcessor.transform.position = newProcessorPosition;
			row.Add(newProcessor);
		}

		if(levelData.levelTop+1 == levelData.newSectionEnd)
		{
			//activate the controller
			int patternVariant = Random.Range(1,4);
			newProcessorGroupController = poolManager.retrieveObject("ProcessorGroup");
			newProcessorGroupController.GetComponent<ProcessorGroupController>().initialize(processorGroup, patternVariant);
			row.Add(newProcessorGroupController);
		}
		

	}

}
