using UnityEngine;
using System.Collections;

public class FireBallController : MonoBehaviour {
    FireBallController myBeetleController;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        Explode();
    }

    private void Explode()
    {
        Debug.Log("Boom");
    }
}
