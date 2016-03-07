using UnityEngine;
using System.Collections;

[System.Serializable]
public class DifficultyParameter{
	public float min;
	public float max;
	public float current;
	
	public void scaleCurrent(float percent)
	{
		current = min + (max - min) * percent;
	}
}
