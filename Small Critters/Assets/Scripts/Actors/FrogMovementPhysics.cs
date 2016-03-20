using UnityEngine;
using System.Collections;
using System;

public class FrogMovementPhysics : MonoBehaviour, Imovement {
	public GameController gameController;
	public Rigidbody2D myRigidBody;

	public Vector3 destination;
	public float closeEnoughDistance;
	public float staticDrag;
	public float jumpingDrag;
	public bool midJump{get;set;}
	public float jumpForce;
	public int higestRowReached;
	public Animator myAnimator;
	public float jumpSpeed;
	private IEnumerator jumpTimer;
	public event EventHandler<NewRowReached> NewHighestRowReached;
	//public EventHandler<NewRowReached> newRowReachedHandler;
	public NewRowReached newRowReachedEventArgs;
	
	// Use this for initialization
	void Start () {
		higestRowReached = 0;
		myRigidBody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
		//newRowReachedHandler = NewHighestRowReached;
		newRowReachedEventArgs = new NewRowReached();
		
	}
	
	void OnCollisionEnter()
	{
		if(midJump)
		{
			StopCoroutine(jumpTimer);
			land ();
		}
	}
	
	public void makeMove(Vector3 direction)
	{
		if(!midJump)
		{
			myRigidBody.velocity = Vector3.zero;
			destination = calculateDestination(direction);
			//rotateToDirection(direction);
			myRigidBody.drag = jumpingDrag;
			myRigidBody.AddForce(direction.normalized * jumpForce, ForceMode2D.Impulse);
			midJump = true;
			gameObject.layer = 14;
			myAnimator.SetFloat("JumpSpeed",calculateJumpAnimationSpeed(direction));
			myAnimator.SetBool("Jumping",true);
			jumpTimer = jumpForSeconds(calculateJumpTime(direction));
			StartCoroutine(jumpTimer);
		}

	}
	IEnumerator jumpForSeconds(float jumpTime)
	{
		yield return new WaitForSeconds(jumpTime);
		land();
	}
	
	float calculateJumpAnimationSpeed(Vector3 direction)
	{
		return myRigidBody.velocity.magnitude / direction.magnitude;
	}
	
	float calculateJumpTime(Vector3 direction)
	{
		return direction.magnitude / (jumpForce/myRigidBody.mass);
	}
	
	void land()
	{
		midJump = false;
		myRigidBody.drag = staticDrag;
		myAnimator.SetBool("Jumping",false);
		checkHighestRowReached();
		gameObject.layer = 10;
	}
	
	void checkHighestRowReached()
	{
		if(this.transform.position.y > higestRowReached)
		{
			higestRowReached = (int)this.transform.position.y;
			newRowReachedEventArgs.newRowReached = higestRowReached;
			OnNewHighestRowReached(newRowReachedEventArgs);
		}
	}
	
	public void OnNewHighestRowReached(NewRowReached newRowReachedEventArgs)
	{
		//EventHandler<NewRowReached> newRowReachedHandler = NewHighestRowReached;
		//newRowReachedHandler(this, newRowReachedEventArgs);
		//Debug.Log (NewHighestRowReached);
		if(NewHighestRowReached != null)
		{
			NewHighestRowReached(this, newRowReachedEventArgs);	
		}
	}
	
	public void configure(GameController controller)
	{
		gameController = controller;
		myRigidBody = GetComponent<Rigidbody2D>();
		midJump = false;
		myAnimator = GetComponent<Animator>();
	}
	
	bool reachedDestination()
	{
		return (destination - this.transform.position).magnitude <=closeEnoughDistance ? true : false;
	}
	
	Vector3 calculateDestination(Vector3 direction)
	{
		return this.transform.position + direction;
	}
	
	public void rotateToDirection(Vector3 direction)
	{
		float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
		myRigidBody.rotation = angle - 90f; //0 degrees is up, not right.
	}
}
