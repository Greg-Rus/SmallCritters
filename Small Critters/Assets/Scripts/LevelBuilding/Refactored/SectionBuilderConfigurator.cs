using UnityEngine;
using System.Collections;

public class SectionBuilderConfigurator: ISectionBuilderConfiguration {

	private LevelData levelData; 
	public SectionBuilderConfigurator(LevelData levelData)
	{
		this.levelData = levelData;
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
		levelData.newSectionEnd = levelData.newSectionStart + Random.Range(6,10); //TODO base on levelTop difficulty
	}
	
	private void ProcessorConfig()
	{
		levelData.newSectionEnd = levelData.newSectionStart + Random.Range(4,7); //TODO base on levelTop difficulty
	}
	
	private void ClearConfig()
	{
		levelData.newSectionEnd = levelData.newSectionStart; //TODO base on levelTop difficulty (or maybe not?). One clear row for now
	}
	
}
