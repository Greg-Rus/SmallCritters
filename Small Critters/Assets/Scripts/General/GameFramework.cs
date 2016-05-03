using UnityEngine;
using System.Collections;

public class GameFramework {

	public LevelData levelData;
	public DifficultyManager difficultyManager;
	public ArenaBuilder arenaBuilder;
    public ScoreHandler scoreHandler;
    public Transform poolParent;
	GameObjectPoolManager poolManager;
	ISectionBuilderConfiguration sectionBuilderConfigurator;
	ISectionBuilderSelection sectionBuilderSeclector;
	ISectionDesigning sectionDesigner;
	LevelHandler levelHandler;
	ServiceLocator services;
	SectionBuilderClear clearBuilder;
	SectionBuilderBlades bladesBuilder;
	SectionBuilderProcessors processorsBuilder;
	SectionBuilderHeatVent heatVentBuilder;
	SectionBuilderBees sectionBuilderBees;
    SectionBuilderBugs bugsBuilder;
	IRowCleanup rowCleaner;
	
	//public GameFramework( LevelData levelData, DifficultyManager difficultyManager, ArenaBuilder arenaBuilder)
	//{
	//	this.levelData = levelData;
	//	this.difficultyManager = difficultyManager;
	//	this.arenaBuilder = arenaBuilder;
	//}

	public LevelHandler BuildGameFramework()
	{
		SetupServiceLocator();
		
		//levelData = new LevelData();
		poolManager = new GameObjectPoolManager(poolParent);
		arenaBuilder.Setup(levelData, poolManager);
		
		sectionBuilderConfigurator = new SectionBuilderConfigurator(levelData) as ISectionBuilderConfiguration;
		sectionBuilderSeclector = new SectionBuilderSelector(sectionBuilderConfigurator, levelData) as ISectionBuilderSelection;
		
		SetupSectionBuilders();
		
		
		//sectionDesigner = new SectionDesigner(sectionBuilderSeclector, levelData) as ISectionDesigning;
		sectionDesigner = new ArenaSectionDesigner(sectionBuilderSeclector, levelData, arenaBuilder) as ISectionDesigning;
		rowCleaner = new RowCleaner(poolManager);
		levelHandler = new LevelHandler(levelData, sectionDesigner, rowCleaner);
		
		return levelHandler;
	}
	
	private void SetupServiceLocator()
	{
		services = new ServiceLocator ();
		ServiceLocator.addService<IDifficultyBasedBuilderPicking>(difficultyManager);
		ServiceLocator.addService<IBladeSectionDifficulty>(difficultyManager.bladeSectionDifficultyManager);
		ServiceLocator.addService<IProcessorGroupDifficulty>(difficultyManager.processorSectionDifficultyManager);
		ServiceLocator.addService<IHeatVentSectionDifficulty>(difficultyManager.heatVentDifficultyManager);
		ServiceLocator.addService<IBeeSectionDifficulty>(difficultyManager.beeSectionDifficultyManager);
        ServiceLocator.addService<BugsDifficultyManager>(difficultyManager.bugsDifficultyManager);
		ServiceLocator.addService<IProcessorFSM> (new ProcessorFSM ());
		ServiceLocator.addService<IProcessorPatternConfiguration> (new ProcessorPatternConfigurator ());
        ServiceLocator.addService<ScoreHandler>(scoreHandler);
		//ServiceLocator.addService<IArenaBuilding>(arenaBuilder);
	}
	
	private void SetupSectionBuilders()
	{
		clearBuilder = new SectionBuilderClear();
		bladesBuilder = new SectionBuilderBlades(levelData, poolManager);
		processorsBuilder = new SectionBuilderProcessors (levelData, poolManager);
		heatVentBuilder = new SectionBuilderHeatVent(levelData, poolManager);
		//sectionBuilderBees = new SectionBuilderBees(levelData, poolManager);
        bugsBuilder = new SectionBuilderBugs(levelData, poolManager);
		sectionBuilderSeclector.addSectionBuilder(clearBuilder);
		sectionBuilderSeclector.addSectionBuilder(bladesBuilder);
		sectionBuilderSeclector.addSectionBuilder(processorsBuilder);
		sectionBuilderSeclector.addSectionBuilder(heatVentBuilder);
        //sectionBuilderSeclector.addSectionBuilder(sectionBuilderBees);
        sectionBuilderSeclector.addSectionBuilder(bugsBuilder);

        levelData.activeSectionBuilder = clearBuilder;
	}
}
