using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
	[TestFixture]
	[Category("Sample Tests")]
	internal class BasicTests
	{
		GameObject poolParent;
		GameObjectPoolManager poolManager;
		GameObject blade;
		LevelHandler testLevelHandler;
		SectionDesigner testSectionDesigner;
		GameObject dummyObject = new GameObject();
		LevelData testLevelData;
		SectionBuilderClear clearBuilder;
		ServiceLocator serviceLocator;
		mockDifficultyManager difficultyManager;
		mockSectionDesigner mSectionBuilderHndl;
		IRowCleanup rowCleaner;
		
		[SetUp] public void Init()
		{
			poolParent = new GameObject(); //.Instantiate(poolParent, Vector3.zero, Quaternion.identity) as GameObject;
			poolManager = new GameObjectPoolManager(poolParent.transform);
			blade = Resources.Load("Blade") as GameObject;
			poolManager.addPool(blade, 100);
			testLevelData= new LevelData();
			clearBuilder = new SectionBuilderClear();
			testLevelData.activeSectionBuilder = clearBuilder;
			serviceLocator = new ServiceLocator();
			difficultyManager = new mockDifficultyManager();
			ServiceLocator.addService<IBladeSectionDifficulty>(difficultyManager);
			ServiceLocator.addService<IProcessorGroupDifficulty>(difficultyManager);
			rowCleaner = new RowCleaner(poolManager);
			//mSectionBuilderHndl = new mockSectionBuilderHndl(poolManager);
		}
	
		[Test]
		public void PoolManagerTest()
		{
			Assert.IsNotNull(poolManager);
		}
		
		[Test]
		public void LevelBuilderCreation()
		{
			testLevelHandler = new LevelHandler(testLevelData, new mockSectionDesigner(), rowCleaner);
			Assert.IsNotNull(testLevelHandler);
		}
		
		[Test]
		public void NewRowFromLevelBuilder()
		{
			List<GameObject> End = testLevelHandler.level.Peek();
			Assert.True(ReferenceEquals(End,testLevelHandler.level.Peek()));
			//End.Add(dummyObject);
			testLevelHandler.buildNewRow();
			Assert.True (ReferenceEquals( End , testLevelHandler.level.ToArray()[testLevelHandler.level.Count - 1]));
			//Assert.False (testLevelHandler.level.Peek().Contains(dummyObject));
			//Assert.True (testLevelHandler.level.ToArray()[testLevelHandler.level.Count - 1].Contains(dummyObject)); 
			
			
		}
		
		[Test]
		public void SectionDesignerCreation()
		{
			testSectionDesigner = new SectionDesigner( new mockSectionBuilderSelector(new mockSectionBuilderConfigurator(), testLevelData), testLevelData);
			Assert.IsNotNull(testSectionDesigner);
			Assert.IsNotNull(testLevelData.activeSectionBuilder);
			Assert.True(testLevelData.newSectionEnd == 0);
		}
		
		[Test]
		public void SectionDesignerNewRowTest()
		{
			List<GameObject> testRow = new List<GameObject>();
			testLevelData.activeSectionBuilder = new mockSectionBuilder();
			Assert.IsNotNull(testLevelData.activeSectionBuilder);
			testSectionDesigner = new SectionDesigner( new mockSectionBuilderSelector(new mockSectionBuilderConfigurator(), testLevelData), testLevelData);
			testSectionDesigner.buildNewRow(testRow);
			Assert.IsNotNull(testLevelData.activeSectionBuilder);
			Assert.True(testLevelData.levelTop == 1);
		}
		
		[Test]
		public void SectionBuilderSelectorTest()
		{
			SectionBuilderSelector testSectionBuilderSelector = new SectionBuilderSelector(new mockSectionBuilderConfigurator(), testLevelData);
			//Assert.True(testLevelData.activeSectionBuilder == null);
			testSectionBuilderSelector.addSectionBuilder(clearBuilder);
			testSectionBuilderSelector.addSectionBuilder(new mockSectionBuilder());
			testSectionBuilderSelector.selectNewSectionBuilder();
			Assert.True(testLevelData.activeSectionBuilder.type == SectionBuilderType.blade);
		}
		
		[Test]
		public void SectionBuilderConfiguratorTest()
		{
			testLevelData.activeSectionBuilder = new mockSectionBuilder();
			SectionBuilderConfigurator testSBConfigurator = new SectionBuilderConfigurator(testLevelData);
			Assert.True(testLevelData.newSectionEnd == 0 && testLevelData.newSectionStart == 0);

			testSBConfigurator.configureSectionBuilder();
			Assert.False(testLevelData.newSectionEnd == 0 && testLevelData.newSectionStart == 0);
		}
		
		[Test]
		public void OrderOfMagnitydeRounding()
		{
			Debug.Log (Utilities.RoundToNearestOrderOfMagnitude(0.02f,0.1f));
			Assert.True(Utilities.RoundToNearestOrderOfMagnitude(0.02f,0.1f, 0.1f) == 0.1f);
			Assert.True(Utilities.RoundToNearestOrderOfMagnitude(0.28f,0.1f) == 0.3f);
			Assert.True(Utilities.RoundToNearestOrderOfMagnitude(0.98f,0.1f) == 1f);
			Assert.True(Utilities.RoundToNearestOrderOfMagnitude(0.036f,0.01f) == 0.04f);
		}
		
		
		
		[TearDown] public void Dispose()
		{
			GameObject.DestroyImmediate(poolParent);
			GameObject.DestroyImmediate(dummyObject);
		}
		
	}
	
}
