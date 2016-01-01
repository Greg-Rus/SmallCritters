﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum sectionBuilderType {blade, processor};

public class SectionBuilderSelector: ISectionBuilderSelection {

	public Dictionary<sectionBuilderType,ISectionBuilder> availableSectionBuilders;
	private ISectionBuilderConfiguration sectionBuilderConfigurator;
	private LevelData levelData;
	
	public SectionBuilderSelector (ISectionBuilderConfiguration sectionBuilderConfigurator, LevelData levelData)
	{
		this.sectionBuilderConfigurator = sectionBuilderConfigurator;
		this.levelData = levelData;
		availableSectionBuilders = new Dictionary<sectionBuilderType, ISectionBuilder>();
	}
	
	public void addSectionBuilder (ISectionBuilder sectionBuilder)
	{
		Debug.Log ("Builder added: " + sectionBuilder);
		availableSectionBuilders.Add(sectionBuilder.type, sectionBuilder);
	}
	
	public void selectNewSectionBuilder()
	{
		int randomBuilder = Random.Range(0, availableSectionBuilders.Count);
		sectionBuilderType builderType = (sectionBuilderType)randomBuilder;
		Debug.Log ("Builder type selected: " + builderType);
		sectionBuilderType newBuilderType = builderType;//(sectionBuilderType)(Random.Range(0, availableSectionBuilders.Count));
		levelData.activeSectionBuilder = availableSectionBuilders[newBuilderType];
		sectionBuilderConfigurator.configureSectionBuilder();
	}

}
