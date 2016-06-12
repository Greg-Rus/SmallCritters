using UnityEngine;
using System.Collections;

public class BladeRowMovement : MonoBehaviour {
	Rigidbody2D myRigidbody;
	public float speed;
	public HorizontalDirection direction; // -1 or 1
	public Vector3 moveCycleEndPoint;
	public Vector3 moveCycleStartPoint;

	void Awake () {
		myRigidbody = GetComponent<Rigidbody2D>();
	}

	void Update () {
		if(direction == HorizontalDirection.Right && transform.position.x >= moveCycleEndPoint.x ||
		   direction == HorizontalDirection.Left  && transform.position.x <= moveCycleEndPoint.x)
		{
			transform.position = moveCycleStartPoint;
		}
		myRigidbody.MovePosition(transform.position + (Vector3.right * (float)direction * speed * Time.deltaTime));

	}
	
	public void configure(float speed, HorizontalDirection direction, float moveDistance, float offset)
	{
		this.speed = speed;
		this.direction = direction;
		moveCycleEndPoint = transform.position + (Vector3.right * (float)direction * moveDistance);
		moveCycleStartPoint = transform.position;
		myRigidbody.MovePosition(transform.position + (Vector3.right * (float)direction * moveDistance * offset));
	}
}
