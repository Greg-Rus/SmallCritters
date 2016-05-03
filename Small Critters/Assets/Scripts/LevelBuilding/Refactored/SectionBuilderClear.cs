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
		//GameObject empty = new GameObject();
		//empty.name = "Empty";
		//row.Add(empty);
		//Empty row. Nothing to build here
	}
	
	
}
