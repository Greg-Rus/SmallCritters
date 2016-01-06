using UnityEngine;
using System.Collections;

public class SectionBuilderConfigurator: ISectionBuilderConfiguration {

	private LevelData levelData; 
	private IBladeSectionLength bladeSectionLenghtManager;
	private IProcessorSectionLenght processorSectionLenghtManager;
	public SectionBuilderConfigurator(LevelData levelData)
	{
		this.levelData = levelData;
		bladeSectionLenghtManager = ServiceLocator.getService<IBladeSectionLength>();
		processorSectionLenghtManager = ServiceLocator.getService<IProcessorSectionLenght>();
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
		levelData.newSectionEnd = levelData.newSectionStart + processorSectionLenghtManager.GetNewProcessorSectionLenght();
	}
	
	private void ClearConfig()
	{
		levelData.newSectionEnd = levelData.newSectionStart; //TODO base on levelTop difficulty (or maybe not?). One clear row for now
	}
	
}
