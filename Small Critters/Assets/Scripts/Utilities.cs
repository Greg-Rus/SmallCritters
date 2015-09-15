using UnityEngine;
using System.Collections;

public class Utilities : MonoBehaviour {

	public static Quaternion rotationFromForwardToVector(Vector3 vector)
	{
		float angle = Mathf.Atan2(vector.y,vector.x) * Mathf.Rad2Deg;
		return Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
