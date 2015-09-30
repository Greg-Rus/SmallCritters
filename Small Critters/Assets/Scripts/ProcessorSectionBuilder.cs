using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProcessorSectionBuilder : MonoBehaviour {
	private int currentArenaHeight;
	private int newArenaHeight;
	private Vector3 processorPosition = Vector3.zero;
	public GameObject processor;
	private float processorCycleOffset = 0f;
	private float processorCycleStage = 0f;
	public int arenaWidth;
	private ObjectPool processorPool;
	private GameObject newProcessor;
	private GameObject[,] deployedProcessors;
	
	// Use this for initialization
	void Awake () {
		processorPool = new ObjectPool(processor, 100);
	}
	
	
	public LevelSection buildProcessorSegment(int fromRow, int toRow)
	{
		processorCycleStage = 0;
		currentArenaHeight = fromRow;
		newArenaHeight = toRow;
		deployedProcessors = new GameObject[arenaWidth -1, (toRow - fromRow)];
		
		int processorSegmentVariant = Random.Range(0,4);
		if(processorSegmentVariant == 0)
		{
			buildProcessorSegmentVariant(0.34f);
		}
		else if (processorSegmentVariant == 1)
		{
			buildProcessorSegmentVariant(0.26f);
		}
		else if (processorSegmentVariant == 2)
		{
			buildProcessorSegmentVariant(0.20f);
		}
		else if (processorSegmentVariant == 3)
		{
			buildProcessorSegmentVariant(0.15f);
		}
		//Debug.Log ("Arena Start: " + (fromRow) + "Arena End: " + toRow);
		LevelSection newSection = new LevelSection(fromRow, toRow, deployedProcessors, processorPool);
		return newSection;
	}
	public void buildProcessorSegmentVariant(float cycleOffset)
	{
		
		processorCycleOffset = cycleOffset;
		for (int i = currentArenaHeight ; i < newArenaHeight ; i++)
		{
			layNextProcessorRow(i);
		}
	}
	private void layNextProcessorRow(int row)
	{
		for (int i = 1; i < arenaWidth ; i++)
		{
			processorPosition.x = i;
			processorPosition.y = row;
			processorPosition.z = this.transform.position.z;
			newProcessor = processorPool.retrieveObject();
			newProcessor.transform.position = processorPosition;
			//GameObject newProcessor = Instantiate(processor, processorPosition, Quaternion.identity) as GameObject;
			newProcessor.transform.parent = this.transform;
			deployedProcessors[i - 1,row - currentArenaHeight] = newProcessor; //TODO the calculation of indexes is ugly. Improve it.
			processorCycleStage = ((processorCycleStage + processorCycleOffset > 1f) ? 0f : processorCycleStage + processorCycleOffset);
			newProcessor.GetComponent<ProcessorHeater>().setProcessorState(processorCycleStage);
		}
	}
	
}
