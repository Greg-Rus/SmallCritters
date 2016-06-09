using UnityEngine;
using System.Collections;

public class CleanSlateStartup : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        PlayerPrefs.DeleteAll();
	}
	

}
