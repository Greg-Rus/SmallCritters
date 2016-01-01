using UnityEngine;
using System.Collections;

public class TestMixedLevelBuilding : MonoBehaviour {

	LevelData levelData;
	GameObjectPoolManager poolManager;
	ISectionBuilderConfiguration testSectionBuilderConfigurator;
	ISectionBuilderSelection testSectionBuilderSeclector;
	ISectionDesigning testSectionDesigner;
	LevelHandler testLevelHandler;
	private ServiceLocator services;
	// Use this for initialization
	void Start () {
		services = new ServiceLocator ();
		ServiceLocator.addService<IProcessorFSM> (new ProcessorFSM ());
		ServiceLocator.addService<IProcessorPatternConfiguration> (new ProcessorPatternConfigurator (ServiceLocator.getService<IProcessorFSM>()));
		levelData = new LevelData();
		poolManager = new GameObjectPoolManager();
		testSectionBuilderConfigurator = new SectionBuilderConfigurator(levelData) as ISectionBuilderConfiguration;
		testSectionBuilderSeclector = new SectionBuilderSelector(testSectionBuilderConfigurator, levelData) as ISectionBuilderSelection;
		
		//testSectionBuilderSeclector.addSectionBuilder(new SectionBuilderBlades(levelData, poolManager));
		testSectionBuilderSeclector.addSectionBuilder(new SectionBuilderProcessors (levelData, poolManager));
		
		testSectionDesigner = new SectionDesigner(testSectionBuilderSeclector, levelData) as ISectionDesigning;
		testLevelHandler = new LevelHandler(levelData, testSectionDesigner);
		
		for (int i = 0; i < 50; ++i)
		{
			testLevelHandler.buildNewRow();
		}
	}
}
