using UnityEngine;
using System.Collections;

public class CleanSlateStartup : MonoBehaviour {

	void Awake () {
        PlayerPrefs.DeleteAll();
	}
}
