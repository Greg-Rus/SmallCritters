using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestSectionBuilderProcessors : MonoBehaviour {
	List<GameObject> testRow;
	SectionBuilderProcessors testBuilder;
	public LevelData levelData;
	GameObjectPoolManager poolManager;
	GameObject testBladeRow;
	public DifficultyManager difficultyManager;
	public bool firstSectionOK;
	public bool SecondSectionOK;
	private ServiceLocator services;
	// Use this for initialization
	void Start () {
		services = new ServiceLocator ();
		//ServiceLocator.addService<IProcessorFSM> (new ProcessorFSM ());
		ServiceLocator.addService<IProcessorGroupDifficulty>(difficultyManager);
		difficultyManager.levelData = levelData;
		ServiceLocator.addService<IProcessorPatternConfiguration> (new ProcessorPatternConfigurator ());
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
		//levelData.newSectionEnd = 10;
		levelData.newSectionEnd = 17;
		levelData.levelTop = 6;
		
		for(int i = 0; i < 11; ++i)//4; ++i)
		{
			testBuilder.buildNewRow(testRow);	
			levelData.levelTop += 1;
		}
		
		if(testRow.Count == 35 + 1 + 77 + 1)
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
