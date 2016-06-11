using UnityEngine;
using System.Collections;

public class JumpMarkerSensor : MonoBehaviour {

	CircleCollider2D myTrigger;
	public LayerMask hazardousLayer;
    public LayerMask targetLayers;
	
	void Awake ()
	{
		myTrigger = transform.GetComponent<CircleCollider2D>();
	}

    public bool checkForHazardsInLandingZone()
	{
		return myTrigger.IsTouchingLayers(hazardousLayer);
	}
    public bool CheckForTargetsInLandingZone()
    {
        return myTrigger.IsTouchingLayers(targetLayers);
    }
}
