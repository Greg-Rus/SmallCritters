using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestSectionBuilderBlades : MonoBehaviour {
	List<GameObject> testRow;
	SectionBuilderBlades testBuilder;
	LevelData levelData;
	GameObjectPoolManager poolManager;
	// Use this for initialization
	void Start () {
		testRow = new List<GameObject>();
		levelData = new LevelData();
		poolManager = new GameObjectPoolManager();
		testBuilder = new SectionBuilderBlades (levelData, poolManager);
		testBuilder.buildNewRow(testRow);
		
	}
	
}
