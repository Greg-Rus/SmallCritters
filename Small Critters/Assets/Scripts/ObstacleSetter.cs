using UnityEngine;
using System.Collections;

public class ObstacleSetter : MonoBehaviour {
	public int arenaHeight;
	public int arenaWidth;
	//public GameObject lineBlades;
	public BladeSectionBuilder bladeBuilder;
	public GameObject processor;
	//private GameObject nextLineBladesObstacle;
	//private Vector3 obstacleSpawnPosition = Vector3.zero;
	public float processorCycleOffset = 0f;
	private float processorCycleStage = 0f;
	public int minBladeSegmentLenght;
	public int maxBladeSegmentLength;
	public int minProcessorSegmentLenght;
	public int maxProcessorSegmentLength;
	//private int currentArenaHeight;
	private int newArenaHeight;
	public int levelLength;
	public int endOfCurrentLevel;
	// Use this for initialization
	void Awake () {
		bladeBuilder = GetComponent<BladeSectionBuilder>();
	}
	
	// Update is called once per frame
	
	public void configure(int width, int height)
	{
		arenaHeight = height;
		arenaWidth = width+1;
		//endOfCurrentLevelStage = 0;
		bladeBuilder.arenaWidth = arenaWidth;
	}
	
//	public void initialObstacleDeplayment()
//	{
//# if (UNITY_EDITOR)
//		for (int i = 1 ; i<= arenaHeight; i++)
//		{
//			layTestObstacle(i);
//		}
//# else		
//		buildNextLevelStage(0);
//# endif
		
//	}
	
	
	
	public void layTestObstacle(int row)
	{
		layNextProcessorRow(row);
	}
	private void layNextProcessorRow(int row)
	{
		for (int i = 1; i < arenaWidth ; i++)
		{
			Vector3 processorPosition;
			processorPosition.x = i;
			processorPosition.y = row;
			processorPosition.z = this.transform.position.z;
			GameObject newProcessor = Instantiate(processor, processorPosition, Quaternion.identity) as GameObject;
			newProcessor.transform.parent = this.transform;
			
			processorCycleStage = ((processorCycleStage + processorCycleOffset > 1f) ? 0f : processorCycleStage + processorCycleOffset);
			//Debug.Log (processorCycleStage);
			newProcessor.GetComponent<ProcessorHeater>().setProcessorState(processorCycleStage);
		}
	}
	public void buildNextLevelSection() //Decide what the next section will be like and build it.
	{
		
		int segmentType = 0;//Random.Range(0,0); //TODO Hardcoded to 0 for test
		if(segmentType == 0)
		{
			selectNewArenaHeight(minBladeSegmentLenght, maxBladeSegmentLength);
			bladeBuilder.buildBladeSection(arenaHeight+1, newArenaHeight);
		}
		else if(segmentType == 1)
		{
			selectNewArenaHeight(minProcessorSegmentLenght, maxProcessorSegmentLength);
			buildProcessorSegment();
		}
	}
	
	public void buildProcessorSegment()
	{
		int processorSegmentVariant = Random.Range(0,3);
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
		
	}
	public void buildProcessorSegmentVariant(float cycleOffset)
	{
		
		processorCycleOffset = cycleOffset;
		for (int i = arenaHeight ; i < newArenaHeight ; i++)
		{
			layNextProcessorRow(i);
		}
	}
	public void selectNewArenaHeight (int min, int max)
	{
		int newLevelLength = Random.Range (min , max);
		newArenaHeight = arenaHeight + newLevelLength;
	}
	public int buildNextLevel()
	{
		//this.arenaHeight = currentArenaHeight;
		//int i =0;
		while ((arenaHeight <  endOfCurrentLevel + levelLength))// || i == 10)
		{
		//	Debug.Log ("here!");
		buildNextLevelSection();
		arenaHeight = newArenaHeight;
		//	i++;
			Debug.Log (arenaHeight + " expecting > " + endOfCurrentLevel+ " " + levelLength);
		}
		endOfCurrentLevel = arenaHeight;
		//endOfCurrentLevelStage = currentArenaHeight;
		Debug.Log (arenaHeight);
		return arenaHeight;
	}
}
