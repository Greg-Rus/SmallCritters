using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleSetter : MonoBehaviour {
	public int arenaHeight;
	public int arenaWidth;
	public BladeSectionBuilder bladeBuilder;
	public ProcessorSectionBuilder processorBuilder;
	public int minBladeSegmentLenght;
	public int maxBladeSegmentLength;
	public int minProcessorSegmentLenght;
	public int maxProcessorSegmentLength;
	private int newArenaHeight;
	public int levelLength;
	public int endOfCurrentLevel;
	public int dismantleDistance; //distance from maxRowReached at which section is dismanteled.
	private List<LevelSection> sections;
	
	// Use this for initialization
	void Awake () {
		bladeBuilder = GetComponent<BladeSectionBuilder>();
		processorBuilder = GetComponent<ProcessorSectionBuilder>();
		sections = new List<LevelSection>();
	}
	
	public void configure(int width, int height)
	{
		arenaHeight = height;
		arenaWidth = width+1;
		bladeBuilder.arenaWidth = arenaWidth;
		processorBuilder.arenaWidth = arenaWidth;
	}
	
	public void buildNextLevelSection() //Decide what the next section will be like and build it.
	{
		
		int segmentType = Random.Range(0,2);
		if(segmentType == 0)
		{
			selectNewArenaHeight(minBladeSegmentLenght, maxBladeSegmentLength);
			bladeBuilder.buildBladeSection(arenaHeight+1, newArenaHeight);
		}
		else if(segmentType == 1)
		{
			selectNewArenaHeight(minProcessorSegmentLenght, maxProcessorSegmentLength);
			sections.Add (processorBuilder.buildProcessorSegment(arenaHeight+1, newArenaHeight));
		}
	}

	public void selectNewArenaHeight (int min, int max)
	{
		int newLevelLength = Random.Range (min , max+1);
		newArenaHeight = arenaHeight + newLevelLength;
	}
	public int buildNextLevel()
	{

		while ((arenaHeight <  endOfCurrentLevel + levelLength))
		{
			buildNextLevelSection();
			arenaHeight = newArenaHeight;
		}
		endOfCurrentLevel = arenaHeight;
		return arenaHeight;
	}
	public void dismantleLevelSectionsBelowRowReached(int rowReached)
	{
		if(sections.Count > 0 && (rowReached - dismantleDistance) >= sections[0].sectionEnd)
		{
			//Debug.Log ("Dismantling !! RowReached is: " + rowReached + " section end is: " + sections[0].sectionEnd);
			sections[0].dismantleSection();
			sections.RemoveAt(0);
		}
	}
}
