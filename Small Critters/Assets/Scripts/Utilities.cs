using UnityEngine;
using System.Collections;

public class Utilities : MonoBehaviour {
    static MonoBehaviour instance;
    void Awake()
    {
        instance = this;
    }

	public static Quaternion rotationFromForwardToVector(Vector3 vector)
	{
		float angle = Mathf.Atan2(vector.y,vector.x) * Mathf.Rad2Deg;
		return Quaternion.AngleAxis(angle, Vector3.forward);
	}

    public static Quaternion RotationFromUpToVector(Vector3 vector)
    {
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.up);
    }
    //public static bool RollBelowPercent(float pecent)
    //{
    //	float roll = RandomLogger.GetRandomRange (instance, 0f,1f);
    //	if(roll <= pecent)
    //	{
    //		return true;
    //	}
    //	else
    //	{
    //		return false;
    //	}

    //}

    public static float RoundToNearestOrderOfMagnitude(float number, float orderOfMagnitude, float roundingPoint = 0.5f)
	{
		float round = number / orderOfMagnitude;
		float integerPart = (int)round;
		float decimalPart = round - (int)round;
		if(decimalPart >= roundingPoint)
		{
			++integerPart;
		}
		float rounded = integerPart * orderOfMagnitude;
		return rounded;
	}
}
