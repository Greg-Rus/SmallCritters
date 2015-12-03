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
		mockSectionBuilderHndl mSectionBuilderHndl;
		
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
			testLevelHandler = new LevelHandler(mockGameData, new mockSectionBuilderHndl(), new mockSectionLenghtHndl());
			Assert.IsNotNull(testLevelHandler);
		}
		
		[Test]
		public void NewRowFromLevelBuilder()
		{
			GameObject dummyObject = new GameObject();
			List<GameObject> End = testLevelHandler.level.Peek();
			End.Add(dummyObject);
			
			testLevelHandler.buildNewRow();
			
			Assert.False (testLevelHandler.level.Peek().Contains(dummyObject));
			Assert.True (testLevelHandler.level.ToArray()[testLevelHandler.level.Count - 1].Contains(dummyObject)); 
		}
		
		
		
		
		
		
		
		[TearDown] public void Dispose()
		{
			GameObject.DestroyImmediate(poolParent);
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
	
	internal class mockSectionBuilderHndl: ISectionBuilderHandling
	{
		
		public List<GameObject> buildNewRow(List<GameObject> row)
		{
			return row;
		}
	}
	
	internal class mockSectionLenghtHndl: ISectionLenghtHandling
	{
		
	}
}
