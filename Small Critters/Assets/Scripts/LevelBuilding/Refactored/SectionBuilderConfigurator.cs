using UnityEngine;
using System.Collections;

public class SectionBuilderConfigurator: ISectionBuilderConfiguration {

	private LevelData levelData; 
	private IBladeSectionDifficulty bladeSectionLenghtManager;
	private IProcessorGroupDifficulty processorSectionDifficultyManager;
	public SectionBuilderConfigurator(LevelData levelData)
	{
		this.levelData = levelData;
		bladeSectionLenghtManager = ServiceLocator.getService<IBladeSectionDifficulty>();
		processorSectionDifficultyManager = ServiceLocator.getService<IProcessorGroupDifficulty>();
	} 
	
	public void configureSectionBuilder()
	{
		levelData.newSectionStart = levelData.levelTop + 1;
		
		switch (levelData.activeSectionBuilder.type)
		{
		case sectionBuilderType.clear: ClearConfig(); break;
		case sectionBuilderType.blade: BladeConfig(); break;
		case sectionBuilderType.processor: ProcessorConfig(); break;
		}
	}
	
	private void BladeConfig()
	{
		levelData.newSectionEnd = levelData.newSectionStart + bladeSectionLenghtManager.GetNewBladeSectionLenght();
	}
	
	private void ProcessorConfig()
	{
		levelData.newSectionEnd = levelData.newSectionStart + processorSectionDifficultyManager.GetNewProcessorSectionLenght();
	}
	
	private void ClearConfig()
	{
		levelData.newSectionEnd = levelData.newSectionStart; //TODO base on levelTop difficulty (or maybe not?). One clear row for now
	}
	
}
