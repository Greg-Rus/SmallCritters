using UnityEngine;
using System.Collections;
using System;
public enum BeeState { Idle, Following, Charging, Stunned };
public class BeeController : MonoBehaviour {
	Action currentAction;
	Animator myAnimator;
	Rigidbody2D myRigidbody;
    public CircleCollider2D myCollider;
	GameObject frog;
	Vector3 vectorToPlayer;
	Vector3 heading;
	float stateExitTime;
	public float chargeTime;
	public float flySpeed;
	public float chargeSpeed;
	public float chargeDistance;
    public int flyLayer;
    public int groundedLayer;
    public int stunCollisionLayer;

	public float stunTime;
	public BeeState state;
	public float chaseTimeLeft;
	private string currentAnimation;
    public ScoreHandler scoreHandler;
    public DeathParticleSystemHandler particlesHandler;
	private int elapsedUpdates = 0;
    private bool alive = false;

    void Awake () {
		currentAction = StayIdle;
		myAnimator = GetComponent<Animator>();
		myRigidbody = GetComponent<Rigidbody2D>();
    }
	void OnEnable() 
	{
        alive = true;
        currentAction = StayIdle;
		state = BeeState.Idle;
        gameObject.layer = groundedLayer;
    }

	void Update () {
		currentAction();
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.layer == stunCollisionLayer && state == BeeState.Charging)
		{
			StartBeingStunned();
		}
		if (coll.collider.CompareTag("Hazard"))
		{
            Die (coll.collider.name);
		}
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard"))
        {
            Die(other.name);
        }
    }

    private void Die(string causeOfDeath)
	{
        if(alive)
        {
            alive = false;
            particlesHandler.OnDeath(causeOfDeath);
            scoreHandler.EnemyDead(this.gameObject, causeOfDeath);
            SetAnimation("Idle");
            WaitUntillAnimatorResets();
        }
	}
	
	private void StartBeingStunned()
	{
		stateExitTime = Time.timeSinceLevelLoad + stunTime;
		state = BeeState.Stunned;
        gameObject.layer = groundedLayer;

		SetAnimation("Stunned");

		currentAction = StayStunned;
	}
	
	private void StayStunned()
	{
        CheckStateExitConditions();
	}

    public void PlayerDetected(GameObject player)
    {
        frog = player;
        StartFollowingPalyer();
    }

    private void StartFollowingPalyer()
	{
		state = BeeState.Following;
		currentAction = FollowPlayer;
        gameObject.layer = flyLayer;
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
		//Dummy state.
	}
	
	private void SetAnimation(string stringInput){
		if(currentAnimation==stringInput){
		}else{
			if(currentAnimation!=null){
				myAnimator.ResetTrigger(currentAnimation);
			}
			myAnimator.SetTrigger(stringInput);
			currentAnimation=stringInput;
		}
	}

	private void WaitUntillAnimatorResets()
	{
		elapsedUpdates = 0;
		currentAction = DisableAfterNextUpdate;
	}

	private void DisableAfterNextUpdate()
	{
		++elapsedUpdates;
		if (elapsedUpdates == 2) {
            gameObject.SetActive (false);
		}
	}
	
	public void Reset()
	{
		currentAction = StayIdle;
		state = BeeState.Idle;
	}
	
}
