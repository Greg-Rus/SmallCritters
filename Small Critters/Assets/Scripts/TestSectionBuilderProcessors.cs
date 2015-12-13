using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestSectionBuilderProcessors : MonoBehaviour {
	List<GameObject> testRow;
	SectionBuilderProcessors testBuilder;
	LevelData levelData;
	GameObjectPoolManager poolManager;
	GameObject testBladeRow;
	public bool firstSectionOK;
	public bool SecondSectionOK;
	// Use this for initialization
	void Start () {
		testRow = new List<GameObject>();
		levelData = new LevelData();
		poolManager = new GameObjectPoolManager();
		testBuilder = new SectionBuilderProcessors (levelData, poolManager);
		
		levelData.newSectionStart = 1;
		levelData.newSectionEnd = 5;
		
		for(int i = 0; i < 5; ++i)
		{
			testBuilder.buildNewRow(testRow);	
			levelData.levelTop += 1;
		}
		
		if(testRow.Count == 35 + 1)
		{
			firstSectionOK = true;
		}
		else
		{
			firstSectionOK = false;
		}
		
		levelData.newSectionStart = 7;
		levelData.newSectionEnd = 10;
		levelData.levelTop = 6;
		
		for(int i = 0; i < 4; ++i)
		{
			testBuilder.buildNewRow(testRow);	
			levelData.levelTop += 1;
		}
		
		if(testRow.Count == 35 + 1 + 28 + 1)
		{
			SecondSectionOK = true;
		}
		else
		{
			SecondSectionOK = false;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
