using UnityEngine;
using System.Collections;

public class FireBeetleDifficultyManager : MonoBehaviour, IFireBeetleDifficultyManager
{

    public DifficultyParameter fireBeetleChance;

    public DifficultyParameter walkSpeed;
    public DifficultyParameter rotationSpeed;
    public DifficultyParameter attackDistanceMax;
    public DifficultyParameter attackDistanceMin;
    public DifficultyParameter shotCooldownTime;
    public DifficultyParameter maxProjectileRange;

    public float difficultyPercent = 0f;
    public float difficultyPercentStep = 0.01f;

    public bool IsFireBeetlePresent()
    {
        return RandomLogger.RollBelowPercent(this, fireBeetleChance.current);
    }

    public void ScaleDifficulty()
    {
        if (difficultyPercent < 1f)
        {
            difficultyPercent += difficultyPercentStep;
            fireBeetleChance.scaleCurrent(difficultyPercent);
            walkSpeed.scaleCurrent(difficultyPercent);
            rotationSpeed.scaleCurrent(difficultyPercent);
            attackDistanceMax.scaleCurrent(difficultyPercent);
            attackDistanceMin.scaleCurrent(difficultyPercent);
            shotCooldownTime.scaleCurrent(difficultyPercent);
        }
    }

    public float GetWalkSpeed()
    {
        return walkSpeed.current;
    }
    public float GetRotationSpeed()
    {
        return rotationSpeed.current;
    }
    public float GetAttackDistanceMax()
    {
        return attackDistanceMax.current;
    }
    public float GetAttackDistanceMin()
    {
        return attackDistanceMin.current;
    }
    public float GetShotCooldownTime()
    {
        return shotCooldownTime.current;
    }
}
