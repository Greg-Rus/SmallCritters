using UnityEngine;
using System.Collections;

public class ColdFogController : MonoBehaviour {
	public float speed;
	Vector3 newPosition;
	public GameObject frog;
	public float baseSpeed;
	public float speedDivisor;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		modifySpeedBasedOnDistanceToFrog();
		moveUp();
	}
	void modifySpeedBasedOnDistanceToFrog()
	{
		float distanceToFrog = (frog.transform.position - this.transform.position).magnitude;
		speed = distanceToFrog / speedDivisor + baseSpeed;
	}
	
	void moveUp()
	{
		newPosition = this.transform.position + Vector3.up * speed * Time.deltaTime;
		this.transform.position = newPosition;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			other.GetComponent<FrogController>().die();
		}
	}
}
