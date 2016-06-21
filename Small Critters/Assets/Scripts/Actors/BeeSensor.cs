using UnityEngine;
using System.Collections;

public class BeeSensor : MonoBehaviour {
    BeeController myController;

	void Awake ()
    {
        myController = GetComponentInParent<BeeController>();
    }
	
    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.tag == "Player" && myController.data.state == BeeState.Idle)
        {
           myController.PlayerDetected(other.gameObject);
        }
    }
}
