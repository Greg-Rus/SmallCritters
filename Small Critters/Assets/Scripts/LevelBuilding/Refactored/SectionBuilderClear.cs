using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectionBuilderClear : ISectionBuilder {

	public SectionBuilderType type {get;set;}
	
	public SectionBuilderClear()
	{
		this.type = SectionBuilderType.clear;
	}
	
	public void buildNewRow(List<GameObject> row)
	{
		//Empty row. Nothing to build here.
	}
	
	
}
