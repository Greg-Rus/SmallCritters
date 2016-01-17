using UnityEngine;
using System.Collections;

public class BladeRowMovement : MonoBehaviour {
	Rigidbody2D myRigidbody;
	public float speed;
	public float direction; // -1 or 1
	public Vector3 moveCycleEndPoint;
	public Vector3 moveCycleStartPoint;
	// Use this for initialization
	void Awake () {
		myRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		//if((moveCycleEndPoint - transform.position).magnitude <= 0.1f)
		//if(transform.position.x <= moveCycleEndPoint.y + 0.01f || transform.position.x <= moveCycleEndPoint.y - 0.01f)
		if(direction > 0 && transform.position.x >= moveCycleEndPoint.x ||
		   direction < 0 && transform.position.x <= moveCycleEndPoint.x)
		{
			transform.position = moveCycleStartPoint;
		}
		myRigidbody.MovePosition(transform.position + (Vector3.right * direction * speed * Time.deltaTime));

	}
	
	public void configure(float speed, float direction, float moveDistance, float offset)
	{
		this.speed = speed;
		this.direction = direction;
		moveCycleEndPoint = transform.position + (Vector3.right * direction * moveDistance);
		moveCycleStartPoint = transform.position;
		myRigidbody.MovePosition(transform.position + (Vector3.right * direction * moveDistance * offset));
	}
}
