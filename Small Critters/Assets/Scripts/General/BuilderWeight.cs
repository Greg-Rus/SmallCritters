using UnityEngine;
using System.Collections;

[System.Serializable]
public class BuilderWeight
{
	public sectionBuilderType type;
	public float weight;
	public BuilderWeight(sectionBuilderType type, float weight)
	{
		this.type = type;
		this.weight = weight;
	}
}