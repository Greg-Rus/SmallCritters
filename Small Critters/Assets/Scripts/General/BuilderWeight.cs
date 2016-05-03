using UnityEngine;
using System.Collections;

[System.Serializable]
public class BuilderWeight
{
	public SectionBuilderType type;
	public float weight;
	public BuilderWeight(SectionBuilderType type, float weight)
	{
		this.type = type;
		this.weight = weight;
	}
}