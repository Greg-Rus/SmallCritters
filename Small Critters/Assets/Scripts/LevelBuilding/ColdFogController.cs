using UnityEngine;
using System.Collections;

public class ColdFogController : MonoBehaviour {
	public float speed;
	Vector3 newPosition;
	public GameObject frog;
	public float baseSpeed;
	public float speedDivisor;
	public float nextRow = 0;
	public float rowDismantleOffset = 5f;
	public ObstacleSetter myObstacleSetter;
	
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		modifySpeedBasedOnDistanceToFrog();
		moveUp();
		checkFrozenRows();
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
	void checkFrozenRows()
	{
		if(this.transform.position.y - rowDismantleOffset >= nextRow)
		{
			myObstacleSetter.dismantleOldestRow();
			++nextRow;
		}
	}
}
