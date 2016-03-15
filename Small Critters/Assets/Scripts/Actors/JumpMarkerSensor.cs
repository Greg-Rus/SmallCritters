using UnityEngine;
using System.Collections;

public class JumpMarkerSensor : MonoBehaviour {

	CircleCollider2D myTrigger;
	public LayerMask hazardousLayer;
	
	void Awake ()
	{
		myTrigger = transform.GetComponent<CircleCollider2D>();
	}

//	public void reset()
//	{}
	public bool checkForHazardsInLandingZone()
	{
		return myTrigger.IsTouchingLayers(hazardousLayer);//isTouchingHazard();
	}
}
