using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class AnalyticsHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void SendOnGameOverEvent(Dictionary<string, object> summary)
    {
        Analytics.CustomEvent("RunSummary", summary);
    }


    
}
