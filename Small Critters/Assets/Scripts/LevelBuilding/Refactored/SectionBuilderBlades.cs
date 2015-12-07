using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectionBuilderBlades: ISectionBuilder {

	public sectionBuilderType type {get;set;}
	LevelData levelData;
	GameObjectPoolManager poolManager;
	GameObject blade;
	GameObject bladeRow;
	float minBaldeGap = 2f;
	float bladeLength;
	int maxBladeNumber;
	
	public SectionBuilderBlades(LevelData levelData, GameObjectPoolManager poolManager)
	{
		this.levelData = levelData;
		this.poolManager = poolManager;
		type = sectionBuilderType.blade;
		blade = Resources.Load("Blade") as GameObject;
		bladeRow = Resources.Load("BladeRow") as GameObject;
		
		poolManager.addPool(blade, 100);
		poolManager.addPool(bladeRow, 30);
	
		bladeLength = blade.GetComponent<BoxCollider2D>().size.x;
	}
	
	public void buildNewRow(List<GameObject> row)
	{
		GameObject newBladeRow = poolManager.retrieveObject("BladeRow");
		float bladeGap = Random.Range(2,5);
		float direction = Random.Range(0,2) == 1 ? 1f : -1f;
		float speed = Random.Range (1,4);
		int numberOfBlades = (int)(levelData.levelWidth / (bladeLength + bladeGap)) + 1;
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
			newBladeRow.GetComponent<BladeRowMovement>().configure(speed, direction, bladeGap);
		}
	}
}
