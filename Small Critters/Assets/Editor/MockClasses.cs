using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
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
		//ISectionBuilderConfiguration mockSBConfigurator;
		LevelData levelData;
		
		public mockSectionBuilderSelector(ISectionBuilderConfiguration mSBC, LevelData levelData)
		{
			//mockSBConfigurator = mSBC;
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
		public SectionBuilderType type {get;set;}
		
		public mockSectionBuilder()
		{
			type = SectionBuilderType.blade;
		}
		
		public void buildNewRow(List<GameObject> row)
		{
		
		}
	}
	
	public class mockSectionBuilderBlades: ISectionBuilder
	{
		public SectionBuilderType type {get;set;}
		private GameObjectPoolManager poolManager;
		private GameObject blade;
		
		public mockSectionBuilderBlades(GameObjectPoolManager poolManager)
		{
			type = SectionBuilderType.blade;
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
		public SectionBuilderType type {get;set;}
		private GameObjectPoolManager poolManager;
		private GameObject processor;
		
		public mockSectionBuilderProcessors(GameObjectPoolManager poolManager)
		{
			type = SectionBuilderType.processor;
			this.poolManager = poolManager;
			processor = Resources.Load("Processor") as GameObject;
			poolManager.addPool(processor, 100);
		}
		
		public void buildNewRow(List<GameObject> row)
		{
			row.Add(poolManager.retrieveObject("Processor"));
		}
	}
	
	public class mockDifficultyManager: IBladeSectionDifficulty, IProcessorGroupDifficulty
	{
		public int GetNewBladeSectionLenght(){return 6;}
		public float GetBladeGap(){ return 5f;	}
		public bool IsBladeRowEmpty(){return true;}
		public float GetBladeSpeed(){return 2;}
		
		public int GetNewProcessorSectionLenght(){return 4;	}
		public int GetNewProcessorGroupPattern(){return 1;}
		public float GetProcessorPatternCycleOffset(){return 1f;}
		public float[] GetProcessorFSMTimers(){return new float[]{1f};}
		public float GetBladeRowCycleOffset(){return 1f;}
		
	}
}
