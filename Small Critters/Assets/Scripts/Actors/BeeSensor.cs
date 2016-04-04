using UnityEngine;
using System.Collections;

public class BeeSensor : MonoBehaviour {
    BeeController myController;

	void Start () {
        myController = GetComponentInParent<BeeController>();
	}
	
    void OnTriggerEnter2D(Collider2D other)
    {
            //Debug.Log (other.name);
            
            if (other.tag == "Player" && myController.state == BeeState.Idle)
            {
               myController.PlayerDetected(other.gameObject);
            }
        }
}
