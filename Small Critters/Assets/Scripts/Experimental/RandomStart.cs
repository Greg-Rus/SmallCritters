using UnityEngine;
using System.Collections;

public class RandomStart : MonoBehaviour {

	public string seed;
	public int seedInt;
	// Use this for initialization
	void Start () {
		seedInt = seed.GetHashCode ();
		Random.InitState(seedInt);
	}


}
