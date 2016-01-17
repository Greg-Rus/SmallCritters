using UnityEngine;
using System.Collections;

public class HeatVentDifficultyManager : MonoBehaviour {

	public DifficultyManager mainDifficultyManager;
	public int heatVentSectionLengthMin = 5;
	public int heatVentSectionLengthMax = 10;
	
	public float minHeatVentLenght = 2f;
	public float minHeatVentLenghtCap = 4f;
	public float maxHeatVentLenght = 5f;
	public float maxHeatVentLenghtCap = 7f;
	public float lengthScalingStep = 0.1f;
	
	public float chanceForEmptyRowInHeatVentSection = 0.5f;
	
	public float heatVentClosedTime = 1f;
	public float heatVentOpeningTime = 0.4f;
	public float heatVentWarmingUpTime = 1f;
	public float heatVentVentingTime = 1.5f;
	public float heatVentClosingTime = 1f;

	// Use this for initialization
	void Awake () {
	
	}
	public bool IsHeatVentRowEmpty()
	{
		return Utilities.RollBelowPercent(chanceForEmptyRowInHeatVentSection);
	}
	
	public int GetNewHeatVentSectionLenght()
	{
		return UnityEngine.Random.Range(heatVentSectionLengthMin, heatVentSectionLengthMax);
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
		return UnityEngine.Random.Range(minHeatVentLenght,maxHeatVentLenght);
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
		if(minHeatVentLenght < minHeatVentLenghtCap)
		{
			minHeatVentLenght += lengthScalingStep;
		}
		if(maxHeatVentLenght < maxHeatVentLenghtCap)
		{
			maxHeatVentLenght += lengthScalingStep;
		}
	}

}
