using UnityEngine;
using System.Collections;

public class TestLevelBuilding : MonoBehaviour {

	LevelData levelData;
	GameObjectPoolManager poolManager;
	ISectionBuilderConfiguration testSectionBuilderConfigurator;
	ISectionBuilderSelection testSectionBuilderSeclector;
	ISectionDesigning testSectionDesigner;
	LevelHandler testLevelHandler;
	SectionBuilderClear clearBuilder;
	SectionBuilderBlades bladesBuilder;
	ServiceLocator services;
	DifficultyManager difficultyManager;
	// Use this for initialization
	void Start () {
		levelData = new LevelData();
		poolManager = new GameObjectPoolManager();
		services = new ServiceLocator();
		difficultyManager = new DifficultyManager();
		testSectionBuilderConfigurator = new SectionBuilderConfigurator(levelData) as ISectionBuilderConfiguration;
		testSectionBuilderSeclector = new SectionBuilderSelector(testSectionBuilderConfigurator, levelData) as ISectionBuilderSelection;
		ServiceLocator.addService<IBladeSectionLength>(difficultyManager);
		ServiceLocator.addService<IProcessorSectionLenght>(difficultyManager);
		clearBuilder = new SectionBuilderClear();
		levelData.activeSectionBuilder = clearBuilder;
		bladesBuilder = new SectionBuilderBlades(levelData, poolManager);
		testSectionBuilderSeclector.addSectionBuilder(clearBuilder);
		testSectionBuilderSeclector.addSectionBuilder(bladesBuilder);
		testSectionDesigner = new SectionDesigner(testSectionBuilderSeclector, levelData) as ISectionDesigning;
		testLevelHandler = new LevelHandler(levelData, testSectionDesigner);
		
		for (int i = 0; i < 50; ++i)
		{
			testLevelHandler.buildNewRow();
		}
	}

}
