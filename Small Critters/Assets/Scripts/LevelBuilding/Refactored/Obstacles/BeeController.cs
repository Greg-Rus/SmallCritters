using UnityEngine;
using System.Collections;
using System;

public class BeeController : MonoBehaviour {
	Action currentAction;
	Animator myAnimator;
	Rigidbody2D myRigidbody;
	GameObject frog;
	Vector3 vectorToPlayer;
	Vector3 heading;
	float stateExitTime;
	public GameObject stunStars;
	public float chargeTime;
	public float flySpeed;
	public float chargeSpeed;
	public float chargeDistance;
	//public float chargeRecoverVelocity;
	public float stunTime;
	public BeeState state;
	public float chaseTimeLeft;
	public GameObjectPoolManager poolManager;
	private string currentAnimation;
	
	public enum BeeState {Idle, Following, Charging, Stunned};
	// Use this for initialization
	void Start () {
		currentAction = StayIdle;
		myAnimator = GetComponent<Animator>();
		myRigidbody = GetComponent<Rigidbody2D>();
	}
	void OnEnable() 
	{
		currentAction = StayIdle;
		state = BeeState.Idle;
	}
	
	// Update is called once per frame
	void Update () {
		currentAction();
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.layer == 12) //12 is Obstacle
		{
			StartBeingStunned();
		}
		if (coll.collider.tag == "Hazard")
		{
			Die ();
		}
	}
	
	private void Die()
	{
		gameObject.SetActive(false);
//		if(poolManager != null)
//		{
//			gameObject.SetActive(false);
//		}
//		else
//		{
//			Debug.LogError ("!!!Tried to clean up Bee, but pool Manager is null!!!");
//			Destroy (gameObject);
//		}
	}
	
	private void StartBeingStunned()
	{
		stateExitTime = Time.timeSinceLevelLoad + stunTime;
		stunStars.SetActive(true);
		state = BeeState.Stunned;
		SetAnimation("Stunned");
//		myAnimator.ResetTrigger("Charge");
//		myAnimator.ResetTrigger("Fly");
//		myAnimator.SetTrigger("Stunned");
		currentAction = StayStunned;
	}
	
	private void StayStunned()
	{
		if(CheckStateExitConditions())
		{
			stunStars.SetActive(false);
		}
	}
	
	
	
	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log (other.name);
		if(other.tag == "Player" && state == BeeState.Idle)
		{
			frog = other.gameObject;
			StartFollowingPalyer();
		}
		
	}
	
	private void StartFollowingPalyer()
	{
		state = BeeState.Following;
		currentAction = FollowPlayer;
//		myAnimator.ResetTrigger("Charge");
//		myAnimator.ResetTrigger("Stunned");
//		myAnimator.SetTrigger("Fly");
		SetAnimation("Fly");
	}
	
	private void FollowPlayer()
	{
		UpdatePlayerLocation();
		RotateToFacePlayer();
		myRigidbody.AddForce(heading * flySpeed);
		if(vectorToPlayer.magnitude <= chargeDistance)
		{
			StartChargingAtPlayer();
		}
		
	}
	


	public void StartChargingAtPlayer()
	{
		RotateToFacePlayer();
		state = BeeState.Charging;
		currentAction = Charge;
		//myAnimator.SetTrigger("Charge");
		SetAnimation("Charge");
		stateExitTime = Time.timeSinceLevelLoad + chargeTime;
		myRigidbody.velocity = Vector3.zero;
	}
	
	public void Charge()
	{
		myRigidbody.AddForce(heading * chargeSpeed);
		chaseTimeLeft = stateExitTime - Time.timeSinceLevelLoad;
		CheckStateExitConditions();
		
	}
	
	private bool CheckStateExitConditions()
	{
		if(Time.timeSinceLevelLoad >= stateExitTime)
		{
			UpdatePlayerLocation();
			if(vectorToPlayer.magnitude <= chargeDistance)
			{
				StartChargingAtPlayer();
			}
			else StartFollowingPalyer();
			return true;
		}
		return false;
	}
	
	public void UpdatePlayerLocation()
	{
		vectorToPlayer = frog.transform.position - this.transform.position;
		heading = vectorToPlayer.normalized;
	}
	
	private void RotateToFacePlayer()
	{
		float angle = Mathf.Atan2(heading.y,heading.x) * Mathf.Rad2Deg;
		myRigidbody.MoveRotation(angle);
	}
	
	private void StayIdle()
	{
		//Dummy state behaviour to put in currentAction
	}
	
	private void SetAnimation(string stringInput){
		//Animator anim =transform.GetComponent<Animator>();
		//Debug.Log("Setting Anim to : " + stringInput);
		if(currentAnimation==stringInput){
		}else{
			if(currentAnimation!=null){
				myAnimator.ResetTrigger(currentAnimation);
			}
			myAnimator.SetTrigger(stringInput);
			currentAnimation=stringInput;
		}
	}
	
	public void Reset()
	{
		transform.eulerAngles = Vector3.zero;
		currentAction = StayIdle;
		state = BeeState.Idle;
	}
	
}
