using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectionBuilderClear : ISectionBuilder {

	public sectionBuilderType type {get;set;}
	
	public SectionBuilderClear()
	{
		this.type = sectionBuilderType.clear;
	}
	
	public void buildNewRow(List<GameObject> row)
	{
		//Empty row. Nothing to build here
	}
}
