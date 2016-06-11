using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityTest;


public class TestSectionBuilderBlades : MonoBehaviour {
	List<GameObject> testRow;
	SectionBuilderBlades testBuilder;
	LevelData levelData;
	GameObjectPoolManager poolManager;
	GameObject testBladeRow;
	// Use this for initialization
	void Start () {
		testRow = new List<GameObject>();
		levelData = new LevelData();
		poolManager = new GameObjectPoolManager();
		testBuilder = new SectionBuilderBlades (levelData, poolManager);
		
		
		for (int i = 0 ; i < 50 ; ++i)
		{
			levelData.levelTop+=1;
			testBuilder.buildNewRow(testRow);
		}
		
	}
	
	void Update()
	{
		
	}
	
}
