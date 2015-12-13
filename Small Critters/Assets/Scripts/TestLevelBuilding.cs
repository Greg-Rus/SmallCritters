using UnityEngine;
using System.Collections;

public class TestLevelBuilding : MonoBehaviour {

	LevelData levelData;
	GameObjectPoolManager poolManager;
	ISectionBuilderConfiguration testSectionBuilderConfigurator;
	ISectionBuilderSelection testSectionBuilderSeclector;
	ISectionDesigning testSectionDesigner;
	LevelHandler testLevelHandler;
	// Use this for initialization
	void Start () {
		levelData = new LevelData();
		poolManager = new GameObjectPoolManager();
		testSectionBuilderConfigurator = new SectionBuilderConfigurator(levelData) as ISectionBuilderConfiguration;
		testSectionBuilderSeclector = new SectionBuilderSelector(testSectionBuilderConfigurator, levelData) as ISectionBuilderSelection;
		testSectionBuilderSeclector.addSectionBuilder(new SectionBuilderBlades(levelData, poolManager));
		testSectionDesigner = new SectionDesigner(testSectionBuilderSeclector, levelData) as ISectionDesigning;
		testLevelHandler = new LevelHandler(levelData, testSectionDesigner);
		
		for (int i = 0; i < 50; ++i)
		{
			testLevelHandler.buildNewRow();
		}
	}

}
