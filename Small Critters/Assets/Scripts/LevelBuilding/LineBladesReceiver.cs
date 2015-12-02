using UnityEngine;
using System.Collections;

public class LineBladesReceiver : MonoBehaviour {

	public LineBladesEmitter emitter;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame

	void OnCollisionEnter2D(Collision2D coll)
	{
		if(emitter.isOwnBlade(coll.gameObject))
		{
			coll.gameObject.transform.position = emitter.transform.position;
		}
	}
}
