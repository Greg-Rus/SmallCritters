using UnityEngine;
using System.Collections;

public class SessionStatistics : MonoBehaviour {
    public static int consecutiveRuns = 0;
    public static float totalTimePlayed = 0f;

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(transform.gameObject);
	}

    // Update is called once per frame

}
