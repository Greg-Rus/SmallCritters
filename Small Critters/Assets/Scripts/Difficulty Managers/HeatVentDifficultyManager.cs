using UnityEngine;
using System.Collections;

public class HeatVentDifficultyManager : MonoBehaviour, IHeatVentSectionDifficulty {
	public float difficultyPercent = 0f;
	public float difficultyPercentStep = 0.01f;
	public DifficultyParameter heatVentSectionLength;
	public DifficultyParameter heatVentLenght;
	public DifficultyParameter emptyRowChance;
    public DifficultyParameter closedTimeVariation;
    public DifficultyParameter ventingTimeVariation;
	
	public float heatVentClosedTime = 1f;
	public float heatVentOpeningTime = 0.4f;
	public float heatVentWarmingUpTime = 1f;
	public float heatVentVentingTime = 1.5f;
	public float heatVentClosingTime = 1f;

	public bool IsHeatVentRowEmpty()
	{
		return RandomLogger.RollBelowPercent(this,emptyRowChance.current);
	}
	
	public int GetNewHeatVentSectionLenght()
	{
		return (int)RandomLogger.GetRandomRange(this,heatVentSectionLength.min, heatVentSectionLength.current);
	}
	
	public float[] GetHeatVentFSMTimers()
	{
		float[] timers = new float[]{
			heatVentClosedTime + RandomLogger.GetRandomRange(this, closedTimeVariation.min, closedTimeVariation.max), 
			heatVentOpeningTime, 
			heatVentWarmingUpTime, 
			heatVentVentingTime + RandomLogger.GetRandomRange(this, ventingTimeVariation.min, ventingTimeVariation.max), 
			heatVentClosingTime};
		return timers;
	}
	
	public float GetHeatVentLength()
	{
		return RandomLogger.GetRandomRange(this,heatVentLenght.min,heatVentLenght.current);
	}
	
	public float GetHeatVentCycleOffset()
	{
		return RandomLogger.GetRandomRange(this,0f,1f);
	}
	
	public Vector3 GetHeatVentRotation()
	{
		if(RandomLogger.RollBelowPercent(this,0.5f))
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
