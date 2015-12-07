using UnityEngine;
using System.Collections;

public class BladeRowMovement : MonoBehaviour {
	Rigidbody2D myRigidbody;
	public float speed;
	public float direction; // -1 or 1
	public Vector3 moveCycleEndPoint;
	public Vector3 moveCycleStartPoint;
	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y >= moveCycleEndPoint.y)
		//TODO so... Hello my future self. Gonna take a break to play BF with Tomek. Anyway the thing is that I need to start the row movement from 0 or level width and then make a
		//proper bound excess check. Looks like the cycle points are correct but you might want to take a look at them. I'm a bit tired now so I'm gonna go. Altogether quite a decent
		//peace of work did here today :)
		{
			transform.position = moveCycleStartPoint;
		}
		myRigidbody.MovePosition(transform.position + (Vector3.right * direction * speed * Time.deltaTime));

	}
	
	public void configure(float speed, float direction, float moveDistance)
	{
		this.speed = speed;
		this.direction = direction;
		moveCycleEndPoint = transform.position + (Vector3.right * direction * moveDistance);
		moveCycleStartPoint = transform.position;
	}
}
