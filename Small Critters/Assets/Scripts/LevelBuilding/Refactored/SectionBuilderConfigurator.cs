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
		case sectionBuilderType.blade: bladeConfig(); break;
		case sectionBuilderType.processor: processorConfig(); break;
		}
	}
	
	private void bladeConfig()
	{
		levelData.newSectionEnd = Random.Range(6,10);
	}
	
	private void processorConfig()
	{
		levelData.newSectionEnd = Random.Range(4,7);
	}
	
}
