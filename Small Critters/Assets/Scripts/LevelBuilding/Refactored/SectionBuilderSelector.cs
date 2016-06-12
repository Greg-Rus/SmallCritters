using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SectionBuilderSelector: ISectionBuilderSelection {

	public Dictionary<SectionBuilderType, ISectionBuilder> availableSectionBuilders;
	private ISectionBuilderConfiguration sectionBuilderConfigurator;
	private LevelData levelData;
	private IDifficultyBasedBuilderPicking difficultyManager;
    private SectionBuilderType newBuilderType;


    public SectionBuilderSelector (ISectionBuilderConfiguration sectionBuilderConfigurator, LevelData levelData)
	{
		this.sectionBuilderConfigurator = sectionBuilderConfigurator;
		this.levelData = levelData;
		difficultyManager = ServiceLocator.getService<IDifficultyBasedBuilderPicking>();
		availableSectionBuilders = new Dictionary<SectionBuilderType, ISectionBuilder>();
	}
	
	public void addSectionBuilder (ISectionBuilder sectionBuilder)
	{
		availableSectionBuilders.Add(sectionBuilder.type,sectionBuilder);
	}
	
	public void selectNewSectionBuilder()
	{
		if(levelData.activeSectionBuilder.type != SectionBuilderType.clear)
		{
            difficultyManager.BanSectionType(levelData.activeSectionBuilder.type);
            newBuilderType = SectionBuilderType.clear;
		}
		else
		{
			newBuilderType = difficultyManager.GetSectionBuilder();
		}
		
		levelData.activeSectionBuilder = availableSectionBuilders[newBuilderType];
		sectionBuilderConfigurator.configureSectionBuilder();
	}

    private void RetryIfSameBuilderSelected()
    {
        if (newBuilderType == levelData.activeSectionBuilder.type)
        {
            newBuilderType = difficultyManager.GetSectionBuilder();
        }
    }

}
