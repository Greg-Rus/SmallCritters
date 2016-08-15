using UnityEngine;
using System.Collections;
using System;

public class FrogMovementPhysics : MonoBehaviour, Imovement {
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
	public event EventHandler<NewRowReached> NewHighestRowReached;
	public NewRowReached newRowReachedEventArgs;

    private IAudio myAudio;
    private IEnumerator jumpTimer;

    void Start () {
		higestRowReached = 0;
		myRigidBody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
        myAudio = ServiceLocator.getService<IAudio>();
        newRowReachedEventArgs = new NewRowReached();
        midJump = false;
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
			myRigidBody.drag = jumpingDrag;
			myRigidBody.AddForce(direction.normalized * jumpForce, ForceMode2D.Impulse);
			midJump = true;
			gameObject.layer = LayerMask.NameToLayer("MidJump");
			myAnimator.SetFloat("JumpSpeed",CalculateJumpAnimationSpeed(direction));
			myAnimator.SetBool("Jumping",true);
			jumpTimer = jumpForSeconds(calculateJumpTime(direction));
			StartCoroutine(jumpTimer);
            myAudio.PlaySound(Sound.Jump);
		}
	}
	IEnumerator jumpForSeconds(float jumpTime)
	{
		yield return new WaitForSeconds(jumpTime);
		land();
	}
	
	float CalculateJumpAnimationSpeed(Vector3 direction)
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
		gameObject.layer = LayerMask.NameToLayer("Hero");
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
		if(NewHighestRowReached != null)
		{
			NewHighestRowReached(this, newRowReachedEventArgs);	
		}
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
