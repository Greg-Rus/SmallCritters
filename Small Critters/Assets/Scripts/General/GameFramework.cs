﻿using UnityEngine;
using System.Collections;

public class GameFramework {

	public LevelData levelData;
	public DifficultyManager difficultyManager;
	public ArenaBuilder arenaBuilder;
    public ScoreHandler scoreHandler;
    public Transform poolParent;
    public PowerupHandler powerupHandler;
    public SoundController soundController;
    public UIHandler UI;
	GameObjectPoolManager poolManager;
	ISectionBuilderConfiguration sectionBuilderConfigurator;
	ISectionBuilderSelection sectionBuilderSeclector;
	ISectionDesigning sectionDesigner;
	LevelHandler levelHandler;
	SectionBuilderClear clearBuilder;
	SectionBuilderBlades bladesBuilder;
	SectionBuilderProcessors processorsBuilder;
	SectionBuilderHeatVent heatVentBuilder;
    SectionBuilderBugs bugsBuilder;
	IRowCleanup rowCleaner;
	
	public LevelHandler BuildGameFramework()
	{
        difficultyManager.levelData = levelData;

        SetupServiceLocator();
		
		poolManager = new GameObjectPoolManager(poolParent);
		arenaBuilder.Setup(levelData, poolManager);
		
		sectionBuilderConfigurator = new SectionBuilderConfigurator(levelData) as ISectionBuilderConfiguration;
		sectionBuilderSeclector = new SectionBuilderSelector(sectionBuilderConfigurator, levelData) as ISectionBuilderSelection;
		
		SetupSectionBuilders();

		sectionDesigner = new ArenaSectionDesigner(sectionBuilderSeclector, levelData, arenaBuilder) as ISectionDesigning;
		rowCleaner = new RowCleaner(poolManager);
		levelHandler = new LevelHandler(levelData, sectionDesigner, rowCleaner);
		
		return levelHandler;
	}
	
	private void SetupServiceLocator()
	{
        new ServiceLocator ();
		ServiceLocator.addService<IDifficultyBasedBuilderPicking>(difficultyManager);
		ServiceLocator.addService<IBladeSectionDifficulty>(difficultyManager.bladeSectionDifficultyManager);
		ServiceLocator.addService<IProcessorGroupDifficulty>(difficultyManager.processorSectionDifficultyManager);
		ServiceLocator.addService<IHeatVentSectionDifficulty>(difficultyManager.heatVentDifficultyManager);
		ServiceLocator.addService<IBeeSectionDifficulty>(difficultyManager.bugsDifficultyManager.beeDifficultyManager);
        ServiceLocator.addService<IFireBeetleDifficultyManager>(difficultyManager.bugsDifficultyManager.fireBeetleDificultyManager);
        ServiceLocator.addService<IBugsSectionDifficulty>(difficultyManager.bugsDifficultyManager);
		ServiceLocator.addService<IProcessorFSM> (new ProcessorFSM ());
		ServiceLocator.addService<IProcessorPatternConfiguration> (new ProcessorPatternConfigurator ());
        ServiceLocator.addService<IDeathReporting>(scoreHandler);
        ServiceLocator.addService<IGameProgressReporting>(scoreHandler);
        ServiceLocator.addService<IScoreForUI>(scoreHandler);
        ServiceLocator.addService<IPowerup>(powerupHandler);
        ServiceLocator.addService<IAudio>(soundController);
        ServiceLocator.addService<IUI>(UI);
    }
	
	private void SetupSectionBuilders()
	{
		clearBuilder = new SectionBuilderClear();
		bladesBuilder = new SectionBuilderBlades(levelData, poolManager);
		processorsBuilder = new SectionBuilderProcessors (levelData, poolManager);
		heatVentBuilder = new SectionBuilderHeatVent(levelData, poolManager);
        bugsBuilder = new SectionBuilderBugs(levelData, poolManager);
		sectionBuilderSeclector.addSectionBuilder(clearBuilder);
		sectionBuilderSeclector.addSectionBuilder(bladesBuilder);
		sectionBuilderSeclector.addSectionBuilder(processorsBuilder);
		sectionBuilderSeclector.addSectionBuilder(heatVentBuilder);
        sectionBuilderSeclector.addSectionBuilder(bugsBuilder);
        levelData.activeSectionBuilder = clearBuilder;
	}
}
