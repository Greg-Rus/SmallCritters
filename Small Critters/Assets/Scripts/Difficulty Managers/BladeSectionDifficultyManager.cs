using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BladeSectionDifficultyManager : MonoBehaviour , IBladeSectionDifficulty{

	public float difficultyPercent = 0f;
	public float difficultyPercentStep = 0.01f;
	public DifficultyParameter bladeSpeed;
	public DifficultyParameter bladeSectionLength;
	public DifficultyParameter bladeGap;
	public DifficultyParameter emptyRowChance;
	public RectTransform BladePanel;
	
	public bool IsBladeRowEmpty()
	{
		return RandomLogger.RollBelowPercent(this,emptyRowChance.current);
	}
	public float GetBladeSpeed()
	{
		return RandomLogger.GetRandomRange(this,bladeSpeed.min, bladeSpeed.current);
	}
	public int GetNewBladeSectionLenght()
	{
		return (int)RandomLogger.GetRandomRange(this,bladeSectionLength.min, bladeSectionLength.current);
	}
	public float GetBladeGap()
	{
		return RandomLogger.GetRandomRange(this,bladeGap.min, bladeGap.current);
	}
	public float GetBladeRowCycleOffset()
	{
		return RandomLogger.GetRandomRange(this,0f,1f);
	}
	
	public void ScaleDifficulty()
	{
		if(difficultyPercent< 1f)
		{
			difficultyPercent += difficultyPercentStep;
			bladeSpeed.scaleCurrent(difficultyPercent);
			bladeSectionLength.scaleCurrent(difficultyPercent);
			bladeGap.scaleCurrent(difficultyPercent);
			emptyRowChance.scaleCurrent(difficultyPercent);
		}
	}
}
