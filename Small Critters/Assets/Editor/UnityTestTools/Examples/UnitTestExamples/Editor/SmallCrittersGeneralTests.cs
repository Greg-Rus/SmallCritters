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
		MockGameData mockGameData;
		LevelHandler testLevelHandler;
		SectionDesigner testSectionDesigner;
		GameObject dummyObject = new GameObject();
		LevelData testLevelData= new LevelData();
		//mockSectionDesigner mSectionBuilderHndl;
		
		[SetUp] public void Init()
		{
			poolParent = new GameObject(); //.Instantiate(poolParent, Vector3.zero, Quaternion.identity) as GameObject;
			poolManager = new GameObjectPoolManager(poolParent.transform);
			blade = Resources.Load("Blade") as GameObject;
			poolManager.addPool(blade, 100);
			mockGameData = new MockGameData();
			
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
			testLevelHandler = new LevelHandler(mockGameData, new mockSectionDesigner());
			Assert.IsNotNull(testLevelHandler);
		}
		
		[Test]
		public void NewRowFromLevelBuilder()
		{
			List<GameObject> End = testLevelHandler.level.Peek();
			End.Add(dummyObject);
			
			testLevelHandler.buildNewRow();
			
			Assert.False (testLevelHandler.level.Peek().Contains(dummyObject));
			Assert.True (testLevelHandler.level.ToArray()[testLevelHandler.level.Count - 1].Contains(dummyObject)); 
			
			
		}
		
		[Test]
		public void SectionDesignerCreation()
		{
			testSectionDesigner = new SectionDesigner( new mockSectionBuilderSelector(new mockSectionBuilderConfigurator()), testLevelData);
			Assert.IsNotNull(testSectionDesigner);
			Assert.IsNotNull(testLevelData.activeSectionBuilder);
			Assert.True(testLevelData.activeSectionBuilder.toRow == 0);
		}
		
		[Test]
		public void SectionDesignerNewRowTest()
		{
			List<GameObject> testRow = new List<GameObject>();
			testSectionDesigner.buildNewRow(testRow);
			Assert.IsNotNull(testRow);
			Assert.True(testLevelData.levelTop == 1);
			//test
		}
		
		
		
		
		
		[TearDown] public void Dispose()
		{
			GameObject.DestroyImmediate(poolParent);
			GameObject.DestroyImmediate(dummyObject);
		}
		
	}
	
	internal class MockGameData: IGameData
	{
		public int getLevelWidth()
		{
			return 7;
		}
		public int getLevelLeght()
		{
			return 50;
		}
	}
	
	internal class mockSectionDesigner: ISectionDesigning
	{
		
		public List<GameObject> buildNewRow(List<GameObject> row)
		{
			return row;
		}
	}
	
	internal class mockSectionBuilderConfigurator: ISectionBuilderConfiguration
	{
		
	}
	
	internal class mockSectionBuilderSelector: ISectionBuilderSelection
	{
		ISectionBuilderConfiguration mockSBConfigurator;
		public mockSectionBuilderSelector(ISectionBuilderConfiguration mSBC)
		{
			mockSBConfigurator = mSBC;
		}
		
		public ISectionBuilder selectNewSectionBuilder()
		{
			return new mockSectionBuilder();
		}
	}
	
	internal class mockSectionBuilder: ISectionBuilder
	{
		public int fromRow {get;set;}
		public int toRow {get;set;}
		
		public mockSectionBuilder()
		{
			fromRow = 0;
			toRow = 0;
		}
		
		public List<GameObject> buildNewRow(List<GameObject> row)
		{
			return row;
		}
	}
}
