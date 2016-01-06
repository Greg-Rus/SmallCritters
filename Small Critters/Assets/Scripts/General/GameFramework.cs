﻿using UnityEngine;
using System.Collections;

public class GameFramework {

	public LevelData levelData;
	public DifficultyManager difficultyManager;
	GameObjectPoolManager poolManager;
	ISectionBuilderConfiguration sectionBuilderConfigurator;
	ISectionBuilderSelection sectionBuilderSeclector;
	ISectionDesigning sectionDesigner;
	LevelHandler levelHandler;
	ServiceLocator services;
	SectionBuilderClear clearBuilder;
	SectionBuilderBlades bladesBuilder;
	SectionBuilderProcessors processorsBuilder;
	IRowCleanup rowCleaner;
	
	public GameFramework( LevelData levelData, DifficultyManager difficultyManager)
	{
		this.levelData = levelData;
		this.difficultyManager = difficultyManager;
	}

	public LevelHandler BuildGameFramework()
	{
		SetupServiceLocator();
		
		//levelData = new LevelData();
		poolManager = new GameObjectPoolManager();
		sectionBuilderConfigurator = new SectionBuilderConfigurator(levelData) as ISectionBuilderConfiguration;
		sectionBuilderSeclector = new SectionBuilderSelector(sectionBuilderConfigurator, levelData) as ISectionBuilderSelection;
		
		SetupSectionBuilders();
		
		sectionDesigner = new SectionDesigner(sectionBuilderSeclector, levelData) as ISectionDesigning;
		rowCleaner = new RowCleaner(poolManager);
		levelHandler = new LevelHandler(levelData, sectionDesigner, rowCleaner);
		
		return levelHandler;
	}
	
	private void SetupServiceLocator()
	{
		services = new ServiceLocator ();
		
		ServiceLocator.addService<IBladeSectionLength>(difficultyManager);
		ServiceLocator.addService<IProcessorSectionLenght>(difficultyManager);
		ServiceLocator.addService<IProcessorGroupPatternDifficulty>(difficultyManager);
		ServiceLocator.addService<IProcessorFSM> (new ProcessorFSM ());
		ServiceLocator.addService<IProcessorPatternConfiguration> (new ProcessorPatternConfigurator ());
	}
	
	private void SetupSectionBuilders()
	{
		clearBuilder = new SectionBuilderClear();
		bladesBuilder = new SectionBuilderBlades(levelData, poolManager);
		processorsBuilder = new SectionBuilderProcessors (levelData, poolManager);
		sectionBuilderSeclector.addSectionBuilder(clearBuilder);
		sectionBuilderSeclector.addSectionBuilder(bladesBuilder);
		sectionBuilderSeclector.addSectionBuilder(processorsBuilder);
		
		levelData.activeSectionBuilder = clearBuilder;
	}
}
