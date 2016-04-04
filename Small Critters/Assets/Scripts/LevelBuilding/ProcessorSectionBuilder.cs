//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class ProcessorSectionBuilder : MonoBehaviour {
//	private int currentArenaHeight;
//	private int newArenaHeight;
//	private Vector3 processorPosition = Vector3.zero;
//	public GameObject processor;
//	private float processorCycleOffset = 0f;
//	private float processorCycleStage = 0f;
//	public int arenaWidth;
//	private GameObjectPoolManager pools;
//	private GameObject newProcessor;
//	private GameObject[] processorRow;
//	public ObstacleSetter myObstacleSetter;
//	private LevelRow newLevelRow;
	
//	// Use this for initialization
//	public void configure (int newArenaWidth, GameObjectPoolManager newPools, ObstacleSetter mySetter) {
//		arenaWidth = newArenaWidth;
//		pools = newPools;
//		pools.addPool(processor, arenaWidth * 50);
//		myObstacleSetter = mySetter;
//	}
	
//	public void buildProcessorSegment(int fromRow, int toRow)
//	{
//		processorCycleStage = 0;
//		currentArenaHeight = fromRow;
//		newArenaHeight = toRow;
		
//		int processorSegmentVariant = Random.Range(0,4);
//		if(processorSegmentVariant == 0)
//		{
//			buildProcessorSegmentVariant(0.34f);
//		}
//		else if (processorSegmentVariant == 1)
//		{
//			buildProcessorSegmentVariant(0.26f);
//		}
//		else if (processorSegmentVariant == 2)
//		{
//			buildProcessorSegmentVariant(0.20f);
//		}
//		else if (processorSegmentVariant == 3)
//		{
//			buildProcessorSegmentVariant(0.15f);
//		}
//		//Debug.Log ("Arena Start: " + (fromRow) + "Arena End: " + toRow);
//		newLevelRow.addHazard(processorRow);
//	}
//	public void buildProcessorSegmentVariant(float cycleOffset)
//	{
		
//		processorCycleOffset = cycleOffset;
//		for (int i = currentArenaHeight ; i < newArenaHeight ; i++)
//		{
//			layNextProcessorRow(i);
//		}
//		myObstacleSetter.addHazardFreeRow(newArenaHeight);
//	}
//	private void layNextProcessorRow(int row)
//	{
//		newLevelRow = myObstacleSetter.getBaseLevelRow(row);
//		processorRow = new GameObject[arenaWidth-1];
//		for (int i = 1; i < arenaWidth ; i++)
//		{
			
//			processorPosition.x = i;
//			processorPosition.y = row;
//			processorPosition.z = this.transform.position.z;
//			newProcessor = pools.retrieveObject("Processor");
//			newProcessor.transform.position = processorPosition;
//			newProcessor.transform.parent = this.transform;
//			processorRow[i-1] = newProcessor;
//			processorCycleStage = ((processorCycleStage + processorCycleOffset > 1f) ? 0f : processorCycleStage + processorCycleOffset);
//			newProcessor.GetComponent<ProcessorHeater>().setProcessorState(processorCycleStage);
//		}
		
//		newLevelRow.addHazard(processorRow);
//		myObstacleSetter.addNewFinishedLevelRow(newLevelRow);
//	}
//}
