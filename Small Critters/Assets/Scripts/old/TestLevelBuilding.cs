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
	DifficultyManager difficultyManager;
	IRowCleanup rowCleaner;
	// Use this for initialization
	void Start () {
		levelData = new LevelData();
		poolManager = new GameObjectPoolManager();
		new ServiceLocator();
		difficultyManager = new DifficultyManager();
		testSectionBuilderConfigurator = new SectionBuilderConfigurator(levelData) as ISectionBuilderConfiguration;
		testSectionBuilderSeclector = new SectionBuilderSelector(testSectionBuilderConfigurator, levelData) as ISectionBuilderSelection;
		ServiceLocator.addService<IBladeSectionDifficulty>(difficultyManager);
		clearBuilder = new SectionBuilderClear();
		levelData.activeSectionBuilder = clearBuilder;
		bladesBuilder = new SectionBuilderBlades(levelData, poolManager);
		testSectionBuilderSeclector.addSectionBuilder(clearBuilder);
		testSectionBuilderSeclector.addSectionBuilder(bladesBuilder);
		testSectionDesigner = new SectionDesigner(testSectionBuilderSeclector, levelData) as ISectionDesigning;
		rowCleaner = new RowCleaner(poolManager);
		testLevelHandler = new LevelHandler(levelData, testSectionDesigner, rowCleaner);
		
		for (int i = 0; i < 50; ++i)
		{
			testLevelHandler.buildNewRow();
		}
	}

}
