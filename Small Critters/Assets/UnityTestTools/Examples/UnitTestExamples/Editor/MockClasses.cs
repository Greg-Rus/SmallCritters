using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
	public class MockGameData: IGameData
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
	
	public class mockSectionDesigner: ISectionDesigning
	{
		
		public void buildNewRow(List<GameObject> row)
		{
			
		}
	}
	
	public class mockSectionBuilderConfigurator: ISectionBuilderConfiguration
	{
		public void configureSectionBuilder()
		{
			
		}
	}
	
	public class mockSectionBuilderSelector: ISectionBuilderSelection
	{
		ISectionBuilderConfiguration mockSBConfigurator;
		LevelData levelData;
		
		public mockSectionBuilderSelector(ISectionBuilderConfiguration mSBC, LevelData levelData)
		{
			mockSBConfigurator = mSBC;
			this.levelData = levelData;
		}
		
		public void addSectionBuilder (ISectionBuilder sectionBuilder)
		{
			//not tested with this object
		}
		
		public void selectNewSectionBuilder()
		{
			levelData.activeSectionBuilder = new mockSectionBuilder();
		}
	}
	
	public class mockSectionBuilder: ISectionBuilder
	{
		public sectionBuilderType type {get;set;}
		
		public mockSectionBuilder()
		{
			type = sectionBuilderType.blade;
		}
		
		public void buildNewRow(List<GameObject> row)
		{
		
		}
	}
	
	public class mockSectionBuilderBlades: ISectionBuilder
	{
		public sectionBuilderType type {get;set;}
		private GameObjectPoolManager poolManager;
		private GameObject blade;
		
		public mockSectionBuilderBlades(GameObjectPoolManager poolManager)
		{
			type = sectionBuilderType.blade;
			this.poolManager = poolManager;
			blade = Resources.Load("Blade") as GameObject;
			poolManager.addPool(blade, 100);
		}
		
		public void buildNewRow(List<GameObject> row)
		{
			row.Add(poolManager.retrieveObject("Blade"));
		}
	}
	
	public class mockSectionBuilderProcessors: ISectionBuilder
	{
		public sectionBuilderType type {get;set;}
		private GameObjectPoolManager poolManager;
		private GameObject processor;
		
		public mockSectionBuilderProcessors(GameObjectPoolManager poolManager)
		{
			type = sectionBuilderType.processor;
			this.poolManager = poolManager;
			processor = Resources.Load("Processor") as GameObject;
			poolManager.addPool(processor, 100);
		}
		
		public void buildNewRow(List<GameObject> row)
		{
			row.Add(poolManager.retrieveObject("Processor"));
		}
	}
}
