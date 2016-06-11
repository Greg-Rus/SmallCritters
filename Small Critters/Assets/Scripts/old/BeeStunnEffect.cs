//using UnityEngine;
//using System.Collections;

//public class BeeStunnEffect : MonoBehaviour {
//	public GameObject[] stars;
//	public float rotationSpeed = 2f;
//	float newAngle = 0;
//	public float minStarScale = 0.5f;
//	public float maxStarScale = 1f;
//	public float scaleStep = 0.1f;
//	float currentStarScale = 1f;
//	float scalingDirection = -1f;
//	// Use this for initialization
//	void Start () {
	
//	}
	
//	// Update is called once per frame
//	void Update () {
//		RotateStars();
//		ScaleStars();
//	}
	
//	private void RotateStars()
//	{
//		//newAngle += Time.deltaTime * rotationSpeed;
//		//if(newAngle >= 360) newAngle = 0;
//		transform.RotateAround(this.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
//	}
	
//	private void ScaleStars()
//	{
//		currentStarScale += scaleStep * scalingDirection;
//		foreach(GameObject star in stars)
//		{
//			star.transform.localScale = Vector3.one * currentStarScale;
//		}
//		if(currentStarScale >= maxStarScale || currentStarScale <= minStarScale) scalingDirection *= -1f;
//	}
//}
