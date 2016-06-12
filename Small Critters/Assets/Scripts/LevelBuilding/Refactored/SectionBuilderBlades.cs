using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectionBuilderBlades: ISectionBuilder {
	
	public SectionBuilderType type {get;set;}
	LevelData levelData;
	GameObjectPoolManager poolManager;
	GameObject blade;
	GameObject bladeRow;
	IBladeSectionDifficulty difficultyManager;
	
	float bladeLength;
	
	public SectionBuilderBlades(LevelData levelData, GameObjectPoolManager poolManager)
	{
		this.levelData = levelData;
		this.poolManager = poolManager;
		difficultyManager = ServiceLocator.getService<IBladeSectionDifficulty>();
		type = SectionBuilderType.blade;
		blade = Resources.Load("Blade") as GameObject;
		bladeRow = Resources.Load("BladeRow") as GameObject;
		
		poolManager.addPool(blade, 100);
		poolManager.addPool(bladeRow, 30);
		
		bladeLength = blade.GetComponent<BoxCollider2D>().size.x;
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
		float bladeGap = difficultyManager.GetBladeGap();
		HorizontalDirection direction = RandomLogger.GetRandomRange(0,2) == 1 ? HorizontalDirection.Right : HorizontalDirection.Left;
		float speed = difficultyManager.GetBladeSpeed();
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
}
