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
	float chargeEndTime;
	public float chargeTime;
	public float flySpeed;
	public float chargeSpeed;
	public float chargeDistance;
	public float chargeRecoverVelocity;
	public BeeState state;
	public float chaseTimeLeft;
	
	public enum BeeState {Idle, Following, Charging};
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
		myAnimator.ResetTrigger("Charge");
		myAnimator.SetTrigger("Fly");
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
		//myAnimator.ResetTrigger("Fly");
		myAnimator.SetTrigger("Charge");
		chargeEndTime = Time.timeSinceLevelLoad + chargeTime;
		myRigidbody.velocity = Vector3.zero;
		//myRigidbody.AddForce(heading * chargePower, ForceMode2D.Impulse);
	}
	
	public void Charge()
	{
		myRigidbody.AddForce(heading * chargeSpeed);
		chaseTimeLeft = chargeEndTime - Time.timeSinceLevelLoad;
		
		if(/*myRigidbody.velocity.magnitude < chargeRecoverVelocity || */Time.timeSinceLevelLoad >= chargeEndTime)
		{
			UpdatePlayerLocation();
			if(vectorToPlayer.magnitude <= chargeDistance)
			{
				Debug.Log(vectorToPlayer.magnitude);
				StartChargingAtPlayer();
			}
			else StartFollowingPalyer();
		}
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
		
	}
}
