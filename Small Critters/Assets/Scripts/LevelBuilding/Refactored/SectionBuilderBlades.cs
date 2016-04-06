using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectionBuilderBlades: ISectionBuilder {
	
	public sectionBuilderType type {get;set;}
	LevelData levelData;
	GameObjectPoolManager poolManager;
	GameObject blade;
	GameObject bladeRow;
	IBladeSectionDifficulty difficultyManager;
	//difficulty based
	//float minBaldeGap = 2f;
	//float maxBladeGap = 5f;
	//float minSpeed = 1f;
	//float maxSpeed = 3f;
	//float chanceForEmptyRow = 0.25f;
	
	float bladeLength;
	int maxBladeNumber;
	
	public SectionBuilderBlades(LevelData levelData, GameObjectPoolManager poolManager)
	{
		this.levelData = levelData;
		this.poolManager = poolManager;
		difficultyManager = ServiceLocator.getService<IBladeSectionDifficulty>();
		type = sectionBuilderType.blade;
		blade = Resources.Load("Blade") as GameObject;
		bladeRow = Resources.Load("BladeRow") as GameObject;
		
		poolManager.addPool(blade, 100);
		poolManager.addPool(bladeRow, 30);
		
		bladeLength = blade.GetComponent<BoxCollider2D>().size.x;
		//TODO 25% should be empty rows?
	}
	
	public void buildNewRow(List<GameObject> row)
	{
        if (!difficultyManager.IsBladeRowEmpty())
        {
            buildNewBladeRow(row);
        }
        else
        {
            levelData.emptyRow = true;
        }
	}
	
	private void buildNewBladeRow(List<GameObject> row)
	{
		float bladeGap = difficultyManager.GetBladeGap(); //Random.Range(minBaldeGap, maxBladeGap) + bladeLength;
		float direction = Random.Range(0,2) == 1 ? 1f : -1f;
		float speed = difficultyManager.GetBladeSpeed();//Random.Range (minSpeed, maxSpeed);
		float moveCycleOffset = difficultyManager.GetBladeRowCycleOffset();
		int numberOfBlades = (int)(levelData.levelWidth / (bladeLength + bladeGap)) + 2;
		
		GameObject newBladeRow = poolManager.retrieveObject("BladeRow");
		row.Add (newBladeRow);
		newBladeRow.transform.position = new Vector3(levelData.levelWidth * 0.5f, (float)levelData.levelTop + 1, 0f);
		
		for(int i = 0 ; i<numberOfBlades ; ++i)
		{
			GameObject newBlade = poolManager.retrieveObject("Blade");
			Vector3 newBladePosition;
			if(direction >0)
			{
				newBladePosition = new Vector3(0f + bladeGap * i,(float)levelData.levelTop + 1, 0f);
			}
			else
			{
				newBladePosition = new Vector3(levelData.levelWidth - bladeGap * i,(float)levelData.levelTop + 1, 0f);
			}
			newBlade.transform.position = newBladePosition;
			newBlade.transform.parent = newBladeRow.transform;
			newBladeRow.GetComponent<BladeRowMovement>().configure(speed, direction, bladeGap, moveCycleOffset);
		
			row.Add (newBlade);
		}
	}
	
//	private bool shouldBeEmptyRow()
//	{
		//TODO decide based on current difficulty (levelTop) 
//		bool isEmpty = (Random.Range(0f,1f)) <= chanceForEmptyRow ? true : false;
//		return isEmpty;
//	}
}
