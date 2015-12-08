using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityTest;


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
		testBuilder.buildNewRow(testRow);
		testBladeRow = testRow[0];
		
	}
	
	void Update()
	{
		
	}
	
}
