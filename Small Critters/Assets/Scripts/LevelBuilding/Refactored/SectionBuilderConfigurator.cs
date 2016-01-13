using UnityEngine;
using System.Collections;

public class SectionBuilderConfigurator: ISectionBuilderConfiguration {

	private LevelData levelData; 
	private IBladeSectionDifficulty bladeSectionLenghtManager;
	private IProcessorGroupDifficulty processorSectionDifficultyManager;
	private IHeatVentSectionDifficulty heatVentSectionDifficultyManager;
	public SectionBuilderConfigurator(LevelData levelData)
	{
		this.levelData = levelData;
		bladeSectionLenghtManager = ServiceLocator.getService<IBladeSectionDifficulty>();
		processorSectionDifficultyManager = ServiceLocator.getService<IProcessorGroupDifficulty>();
		heatVentSectionDifficultyManager = ServiceLocator.getService<IHeatVentSectionDifficulty>();
	} 
	
	public void configureSectionBuilder()
	{
		levelData.newSectionStart = levelData.levelTop + 1;
		
		switch (levelData.activeSectionBuilder.type)
		{
		case sectionBuilderType.clear: ClearConfig(); break;
		case sectionBuilderType.blade: BladeConfig(); break;
		case sectionBuilderType.processor: ProcessorConfig(); break;
		case sectionBuilderType.heatVent: HeatVentConfig(); break;
		default: Debug.LogError("Section Builder Configurator was asked to configure: " + levelData.activeSectionBuilder.type + " ,but functionality not yet implemented"); break;
		}
	}
	private void ClearConfig()
	{
		levelData.newSectionEnd = levelData.newSectionStart; //TODO base on levelTop difficulty (or maybe not?). One clear row for now
	}
	
	private void BladeConfig()
	{
		levelData.newSectionEnd = levelData.newSectionStart + bladeSectionLenghtManager.GetNewBladeSectionLenght();
	}
	
	private void ProcessorConfig()
	{
		levelData.newSectionEnd = levelData.newSectionStart + processorSectionDifficultyManager.GetNewProcessorSectionLenght();
	}
	
	private void HeatVentConfig()
	{
		levelData.newSectionEnd = levelData.newSectionStart + heatVentSectionDifficultyManager.GetNewHeatVentSectionLenght();
	}
	
}
