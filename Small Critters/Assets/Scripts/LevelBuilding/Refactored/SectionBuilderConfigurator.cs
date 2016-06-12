using UnityEngine;
using System.Collections;

public class SectionBuilderConfigurator: ISectionBuilderConfiguration {

	private LevelData levelData; 
	private IBladeSectionDifficulty bladeSectionLenghtManager;
	private IProcessorGroupDifficulty processorSectionDifficultyManager;
	private IHeatVentSectionDifficulty heatVentSectionDifficultyManager;
    private IBugsSectionDifficulty bugsSectionDifficulty;

    public SectionBuilderConfigurator(LevelData levelData)
	{
		this.levelData = levelData;
		bladeSectionLenghtManager = ServiceLocator.getService<IBladeSectionDifficulty>();
		processorSectionDifficultyManager = ServiceLocator.getService<IProcessorGroupDifficulty>();
		heatVentSectionDifficultyManager = ServiceLocator.getService<IHeatVentSectionDifficulty>();
        bugsSectionDifficulty = ServiceLocator.getService<IBugsSectionDifficulty>();

    } 
	
	public void configureSectionBuilder()
	{
		levelData.newSectionStart = levelData.levelTop + 1;
		
		switch (levelData.activeSectionBuilder.type)
		{
		case SectionBuilderType.clear: ClearConfig(); break;
		case SectionBuilderType.blade: BladeConfig(); break;
		case SectionBuilderType.processor: ProcessorConfig(); break;
		case SectionBuilderType.heatVent: HeatVentConfig(); break;
        case SectionBuilderType.bugs: BugsConfig(); break;
		default: Debug.LogError("Section Builder Configurator was asked to configure: " + levelData.activeSectionBuilder.type + " ,but functionality not yet implemented"); break;
		}
	}
	private void ClearConfig()
	{
		levelData.newSectionEnd = levelData.newSectionStart;
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
	
    private void BugsConfig()
    {
        levelData.newSectionEnd = levelData.newSectionStart + bugsSectionDifficulty.GetNewBugSectionLength();
    }
}
