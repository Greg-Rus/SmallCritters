using UnityEngine;
using System.Collections;

public class CameraVerticalFollow : MonoBehaviour {
	public GameObject frog;
	private Vector3 newCameraPosition;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		followFrog();
	}
	private void followFrog()
	{
		if(frog != null)
		{
			newCameraPosition = this.transform.position;
			newCameraPosition.y = frog.transform.position.y;
			this.transform.position = newCameraPosition;
		}

	}
}
