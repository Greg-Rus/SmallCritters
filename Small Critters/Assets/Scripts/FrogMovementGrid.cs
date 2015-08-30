using UnityEngine;
using System.Collections;

public class FrogMovementGrid : MonoBehaviour, Imovement {

	public bool inMotion = false;
	public float JumpSpeed;
	Vector3 destination;
	public Vector3 moveVector;
	Rigidbody2D myRigidBody;
	Animator myAnimator;
	public float moveCompletion;
	Animation jumpAnim;
	public float moveTimer;
	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(inMotion)
		{
			updateMove();
		}
	}
	
	public void updateMove()
	{
		if(closeEnough(destination))
		{
			inMotion = false;
			myRigidBody.velocity = Vector3.zero;
			this.transform.position = destination;
			moveCompletion=100f;
			moveTimer = Time.timeSinceLevelLoad - moveTimer;
			myAnimator.SetBool("Jumping",false);
		}
		else
		{
			myRigidBody.MovePosition(this.transform.position + moveVector * JumpSpeed * Time.deltaTime);
			//updateAnimetionState();
		}
		
		
		
	}
	private void updateAnimetionState()
	{
		moveCompletion = 100f - ((this.transform.position - destination).magnitude * 100f);
		moveCompletion *= 0.01f;
		
	}
	
	public void makeMove(Vector3 direction)
	{
		if(inMotion==false)
		{
			inMotion=true;
			calculateMoveVector(direction);
			calculateDestination();
			turnFrogToMoveVector();
			myAnimator.SetBool("Jumping",true);
			myAnimator.SetFloat("JumpSpeed", JumpSpeed);
			moveTimer = Time.timeSinceLevelLoad;			
		}
		
	}
	private void calculateDestination()
	{
		destination = this.transform.position + moveVector;
	} 
	
	private void calculateMoveVector(Vector3 direction)
	{
		if(Mathf.Abs (direction.x) > Mathf.Abs(direction.y))
		{
			if(direction.x > 0f)
			{
				moveVector = new Vector3(1f,0f,0f);
			}
			else
			{
				moveVector = new Vector3(-1f,0f,0f); //TODO need to implement arena boudery check
			}
		}
		else
		{
			if(direction.y > 0f)
			{
				moveVector = new Vector3(0f,1f,0f);
			}
			else
			{
				moveVector = new Vector3(0f,-1f,0f);
			}
		}
	}
	
	private void turnFrogToMoveVector()
	{
		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, moveVector);
		//this.transform.rotation = Quaternion.Euler(new Vector3 (0f,0f, angle));
		this.transform.rotation = rotation;
	}
	
	private bool closeEnough(Vector3 destination)
	{
		if((destination - this.transform.position).magnitude <= 0.05f)
		{
			return true;
		}
		else
		{
			return false;
		}
		
	}
	

}
