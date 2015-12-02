using UnityEngine;
using System.Collections;

public class JumpMarkerSensor : MonoBehaviour {
	private int hazardInTrigger = 0;

	void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log(other.name);
		{
			++hazardInTrigger;
			//Debug.Log ("Hazards in Trigger "+ hazardInTrigger);
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		{
			--hazardInTrigger;
		}
	}
	public bool isTouchingHazard()
	{
		return (hazardInTrigger > 0) ? true : false;
	}
	public void reset()
	{
		hazardInTrigger = 0;
	}
	public bool checkForHazardsInLandingZone()
	{
		return isTouchingHazard();
	}
}
