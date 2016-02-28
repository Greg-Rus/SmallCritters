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
	public float flySpeed;
	public float chargePower;
	public float chargeDistance;
	public float chargeRecoverVelocity;
	// Use this for initialization
	void Start () {
		currentAction = StayIdle;
		myAnimator = GetComponent<Animator>();
		myRigidbody = GetComponent<Rigidbody2D>();
	}
	void OnEnable() 
	{
		currentAction = StayIdle;
	}
	
	// Update is called once per frame
	void Update () {
		currentAction();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log (other.name);
		if(other.tag == "Player")
		{
			Debug.Log ("Frog detected!");
			frog = other.gameObject;
			StartFollowingPalyer();
		}
		
	}
	
	private void StartFollowingPalyer()
	{
		currentAction = FollowPlayer;
		myAnimator.SetTrigger("Fly");
	}
	
	private void FollowPlayer()
	{
		vectorToPlayer = frog.transform.position - this.transform.position;
		heading = vectorToPlayer.normalized;
		float angle = Mathf.Atan2(heading.y,heading.x) * Mathf.Rad2Deg;
		myRigidbody.MoveRotation(angle);
		myRigidbody.AddForce(heading * flySpeed);
		if(vectorToPlayer.magnitude <= chargeDistance)
		{
			StartChargingAtPlayer();
		}
		
	}

	public void StartChargingAtPlayer()
	{
		currentAction = Charge;
		myAnimator.SetTrigger("Charge");
		myRigidbody.AddForce(heading * chargePower, ForceMode2D.Impulse);
	}
	
	public void Charge()
	{
		//myRigidbody.AddForce(heading * flySpeed);
		if(myRigidbody.velocity.magnitude < chargeRecoverVelocity)
		{
			StartFollowingPalyer();
		}
	}
	
	private void StayIdle()
	{
		
	}
}
