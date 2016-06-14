using UnityEngine;
using System.Collections;

public class BeeSensor : MonoBehaviour {
    BeeFSM myFSM;
    BeeController myController;

	void Awake () {
        myFSM = GetComponentInParent<BeeFSM>();
        myController = GetComponentInParent<BeeController>();

    }
	
    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.tag == "Player" && myFSM.state == BeeState.Idle)
        {
           myController.PlayerDetected(other.gameObject);
        }
    }
}
