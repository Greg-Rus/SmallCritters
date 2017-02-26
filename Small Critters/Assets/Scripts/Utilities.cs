using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Utilities : MonoBehaviour {
    static MonoBehaviour instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
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

    public static void PropagateButtonStateToChildren(Button button)
    {
        Image[] images = button.GetComponentsInChildren<Image>();
        Color targetColor = (button.interactable) ? button.colors.normalColor : button.colors.disabledColor;
        foreach (Image image in images)
        {
            image.color = targetColor;
        }
    }
}
