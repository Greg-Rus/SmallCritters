using UnityEngine;
using System.Collections;

public class HeatVentDifficultyManager : MonoBehaviour, IHeatVentSectionDifficulty {
	public float difficultyPercent = 0f;
	public float difficultyPercentStep = 0.01f;
	public DifficultyParameter heatVentSectionLength;
	public DifficultyParameter heatVentLenght;
	public DifficultyParameter emptyRowChance;
	
	public float heatVentClosedTime = 1f;
	public float heatVentOpeningTime = 0.4f;
	public float heatVentWarmingUpTime = 1f;
	public float heatVentVentingTime = 1.5f;
	public float heatVentClosingTime = 1f;

	public bool IsHeatVentRowEmpty()
	{
		return Utilities.RollBelowPercent(emptyRowChance.current);
	}
	
	public int GetNewHeatVentSectionLenght()
	{
		return (int)UnityEngine.Random.Range(heatVentSectionLength.min, heatVentSectionLength.current);
	}
	
	public float[] GetHeatVentFSMTimers()
	{
		
		float[] timers = new float[]{
			heatVentClosedTime + UnityEngine.Random.Range(-0.5f, 2f), 
			heatVentOpeningTime, 
			heatVentWarmingUpTime, 
			heatVentVentingTime + UnityEngine.Random.Range(-1f, 1.5f), 
			heatVentClosingTime};
		return timers;
	}
	
	public float GetHeatVentLength()
	{
		return UnityEngine.Random.Range(heatVentLenght.min,heatVentLenght.current);
	}
	
	public float GetHeatVentCycleOffset()
	{
		return UnityEngine.Random.Range(0f,1f);
	}
	
	public Vector3 GetHeatVentRotation()
	{
		if(Utilities.RollBelowPercent(0.5f))
		{return Vector3.zero;}
		else
		{return new Vector3(0f,0f,180f);}
	}
	
	public void ScaleDifficulty()
	{
		ScaleFlameLenght();
	}
	
	private void ScaleFlameLenght()
	{
		difficultyPercent += difficultyPercentStep;
		heatVentSectionLength.scaleCurrent(difficultyPercent);
		heatVentLenght.scaleCurrent(difficultyPercent);
		emptyRowChance.scaleCurrent(difficultyPercent);
	}

}
