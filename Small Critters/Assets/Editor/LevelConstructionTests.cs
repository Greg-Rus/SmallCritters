﻿using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
//	[TestFixture]
//	[Category("LevelConstruction")]
	internal class LevelConstructionArchitectureIntegration
	{
		GameObject poolParent;
		GameObjectPoolManager poolManager;
		//GameObject blade;
		//SectionDesigner testSectionDesigner;
		//GameObject dummyObject = new GameObject();
		LevelData testLevelData;
		SectionBuilderClear clearBuilder;
		mockDifficultyManager difficultyManager;
		//mockSectionDesigner mSectionBuilderHndl;
		
		[SetUp] public void Init()
		{
			poolParent = new GameObject(); //.Instantiate(poolParent, Vector3.zero, Quaternion.identity) as GameObject;
			poolManager = new GameObjectPoolManager(poolParent.transform);
			new ServiceLocator();
			difficultyManager = new mockDifficultyManager();
			ServiceLocator.addService<IBladeSectionDifficulty>(difficultyManager);
			ServiceLocator.addService<IProcessorGroupDifficulty>(difficultyManager);
			//blade = Resources.Load("Blade") as GameObject;
			//poolManager.addPool(blade, 100);
			testLevelData= new LevelData();
			clearBuilder = new SectionBuilderClear();
			testLevelData.activeSectionBuilder = clearBuilder;
			//Debug.Log(testLevelData);
		}
		
		[Test]
		public void makeLevelData()
		{ 
			Assert.IsNotNull(testLevelData);
		}
		
		[Test]
		public void makeSBConfigurator()
		{ 
			ISectionBuilderConfiguration testSectionBuilderConfigurator = new SectionBuilderConfigurator(testLevelData) as ISectionBuilderConfiguration;
			Assert.IsNotNull(testSectionBuilderConfigurator);
		}
		
		[Test]
		public void makeSBSelector()
		{ 
			ISectionBuilderConfiguration testSectionBuilderConfigurator = new SectionBuilderConfigurator(testLevelData) as ISectionBuilderConfiguration;
			ISectionBuilderSelection testSectionBuilderSeclector = new SectionBuilderSelector(testSectionBuilderConfigurator, testLevelData) as ISectionBuilderSelection;
			Assert.IsNotNull(testSectionBuilderSeclector);
		}
		[Test]
		public void makeSectionDesigner()
		{ 
			ISectionBuilderConfiguration testSectionBuilderConfigurator = new SectionBuilderConfigurator(testLevelData) as ISectionBuilderConfiguration;
			ISectionBuilderSelection testSectionBuilderSeclector = new SectionBuilderSelector(testSectionBuilderConfigurator, testLevelData) as ISectionBuilderSelection;
			testSectionBuilderSeclector.addSectionBuilder(clearBuilder);
			testSectionBuilderSeclector.addSectionBuilder(new mockSectionBuilderBlades(poolManager));
			testSectionBuilderSeclector.addSectionBuilder(new mockSectionBuilderProcessors(poolManager));
			ISectionDesigning testSectionDesigner = new SectionDesigner(testSectionBuilderSeclector, testLevelData) as ISectionDesigning;
			Assert.IsNotNull(testSectionDesigner);
		}
		
		[Test]
		public void makeLevelHandler()
		{ 
			ISectionBuilderConfiguration testSectionBuilderConfigurator = new SectionBuilderConfigurator(testLevelData) as ISectionBuilderConfiguration;
			ISectionBuilderSelection testSectionBuilderSeclector = new SectionBuilderSelector(testSectionBuilderConfigurator, testLevelData) as ISectionBuilderSelection;
			testSectionBuilderSeclector.addSectionBuilder(clearBuilder);
			testSectionBuilderSeclector.addSectionBuilder(new mockSectionBuilderBlades(poolManager));
			testSectionBuilderSeclector.addSectionBuilder(new mockSectionBuilderProcessors(poolManager));
			ISectionDesigning testSectionDesigner = new SectionDesigner(testSectionBuilderSeclector, testLevelData) as ISectionDesigning;
			IRowCleanup rowCleaner = new RowCleaner(poolManager);
			LevelHandler testLevelHandler = new LevelHandler(testLevelData, testSectionDesigner, rowCleaner);
			Assert.IsNotNull(testLevelHandler);
			//Assert.IsNotNull(testLevelData.activeSectionBuilder);
			//Assert.False(testLevelData.newSectionEnd == 0 && testLevelData.newSectionStart == 0);
//			Assert.True (testLevelData.levelTop == 1);
//			string levelObjectName = testLevelHandler.level.ToArray()[testLevelHandler.level.Count - 1][0].name;
//			Assert.True (levelObjectName == "Blade" || levelObjectName == "Processor"); 
		}
		
		[TearDown] public void Dispose()
		{
			GameObject.DestroyImmediate(poolParent);
			//GameObject.DestroyImmediate(dummyObject);
		}
	}
/*	
	internal class LevelConstruction //Those are not unit tests. They are integration tests untill I mock all but one class or get rid of them.
	{
		GameObject poolParent;
		GameObjectPoolManager poolManager;
		LevelData testLevelData;
		LevelHandler testLevelHandler;
		SectionBuilderClear clearBuilder;
		ServiceLocator services;
		DifficultyManager difficultyManager;
		
		[SetUp] public void Init()
		{
			poolParent = new GameObject(); //.Instantiate(poolParent, Vector3.zero, Quaternion.identity) as GameObject;
			poolManager = new GameObjectPoolManager(poolParent.transform);
			testLevelData= new LevelData();
			difficultyManager = new DifficultyManager();
			services = new ServiceLocator();
			clearBuilder = new SectionBuilderClear();
			
			testLevelData.activeSectionBuilder = clearBuilder;
			ServiceLocator.addService<IBladeSectionLength>(difficultyManager);
			ServiceLocator.addService<IProcessorSectionLenght>(difficultyManager);
			ISectionBuilderConfiguration testSectionBuilderConfigurator = new SectionBuilderConfigurator(testLevelData) as ISectionBuilderConfiguration;
			ISectionBuilderSelection testSectionBuilderSeclector = new SectionBuilderSelector(testSectionBuilderConfigurator, testLevelData) as ISectionBuilderSelection;
			testSectionBuilderSeclector.addSectionBuilder(clearBuilder);
			testSectionBuilderSeclector.addSectionBuilder(new mockSectionBuilderBlades(poolManager));
			testSectionBuilderSeclector.addSectionBuilder(new mockSectionBuilderProcessors(poolManager));
			ISectionDesigning testSectionDesigner = new SectionDesigner(testSectionBuilderSeclector, testLevelData) as ISectionDesigning;
			testLevelHandler = new LevelHandler(testLevelData, testSectionDesigner);
		}
		
		[Test]
		public void buildOneNewRow()
		{ 
			testLevelHandler.buildNewRow();
			Assert.True (testLevelData.levelTop == 1);
			string levelObjectName = testLevelHandler.level.ToArray()[testLevelHandler.level.Count - 1][0].name;
			//Debug.Log (levelObjectName);
			Assert.True (levelObjectName == "Blade" || levelObjectName == "Processor"); 
		}
		
		[Test]
		public void build50NewRows()
		{ 
			for(int i = 0; i<50; ++i)
			{
				testLevelHandler.buildNewRow();
			}
			Assert.True (testLevelData.levelTop == 50);
			if(testLevelHandler.level.ToArray()[testLevelHandler.level.Count - 1].Count > 0)
			{
				string topRowObjectName = testLevelHandler.level.ToArray()[testLevelHandler.level.Count - 1][0].name;
				Assert.True (topRowObjectName == "Blade" || topRowObjectName == "Processor" || topRowObjectName == "Empty");
			}
			else Assert.True (true);
			
			if(testLevelHandler.level.ToArray()[25].Count >0)
			{
				string middleRowObjectName = testLevelHandler.level.ToArray()[25][0].name;
				Assert.True (middleRowObjectName == "Blade" || middleRowObjectName == "Processor" || middleRowObjectName == "Empty"); 
			}
			else Assert.True (true);
			
			string botomRowObjectName = testLevelHandler.level.ToArray()[0][0].name;
			Assert.True (botomRowObjectName == "Blade" || botomRowObjectName == "Processor" || botomRowObjectName == "Empty"); 

			//Assert.True (testLevelHandler.level.ToArray()[testLevelHandler.level.Count - 1].Count == 0);
			
		}
	
	
		[TearDown] public void Dispose()
		{
			GameObject.DestroyImmediate(poolParent);
		}
	}
*/
	internal class ProcessorObstacleTests
	{
		IProcessorFSM testProcessorFSM;
		GameObject testProcessor;
		ProcessorManager testProcessorManager;
		
		[SetUp] public void Init()
		{
			testProcessorFSM = new ProcessorFSM ();
			testProcessor = Resources.Load ("Processor") as GameObject;
			testProcessorManager = testProcessor.GetComponent<ProcessorManager> ();
		}
		
		[Test]
		public void processorCycleSetting20()
		{
			Assert.NotNull (testProcessorManager);
			float time = Time.timeSinceLevelLoad;
			testProcessorFSM.SetCycleCompletion (testProcessorManager,0.2f);
			Assert.True (testProcessorManager.state == ProcessorState.Cool);
			//Assert.True (testProcessorManager.stateExitTime == (time + 0.2f));
			Assert.True(float.Equals (Math.Round( (decimal)testProcessorManager.stateExitTime, 1), Math.Round((decimal)(time + 0.2f),1)));
		}

		[Test]
		public void processorCycleSetting50()
		{
			float time = Time.timeSinceLevelLoad;
			testProcessorFSM.SetCycleCompletion (testProcessorManager,0.5f);
			Assert.True (testProcessorManager.state == ProcessorState.HeatingUp);
			Assert.True(float.Equals (Math.Round( (decimal)testProcessorManager.stateExitTime, 1), Math.Round((decimal)(time + 0.0f),1)));
		}

		[Test]
		public void processorCycleSettingHalfTimeOfHotState()
		{
			float time = Time.timeSinceLevelLoad;
			testProcessorFSM.SetCycleCompletion (testProcessorManager,0.625f);
			Assert.True (testProcessorManager.state == ProcessorState.Hot);
			Assert.True(float.Equals (Math.Round( (decimal)testProcessorManager.stateExitTime, 1), Math.Round((decimal)(time + 0.5f),1)));
		}
		
		[TearDown] public void Dispose()
		{
			//GameObject.DestroyImmediate(testProcessor);
		}
	}
}
