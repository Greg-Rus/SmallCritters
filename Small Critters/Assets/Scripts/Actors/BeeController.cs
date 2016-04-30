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
	//public GameObject stunStars;
	public float chargeTime;
	public float flySpeed;
	public float chargeSpeed;
	public float chargeDistance;
    public int flyLayer;
    public int groundedLayer;
    public int stunCollisionLayer;

	//public float chargeRecoverVelocity;
	public float stunTime;
	public BeeState state;
	public float chaseTimeLeft;
	public GameObjectPoolManager poolManager;
	private string currentAnimation;
    public ScoreHandler scoreHandler;
    public DeathParticleSystemHandler particlesHandler;
	private int elapsedUpdates = 0;
    //public GameObject deathByForceParticles;
    //public GameObject deathByFireParticles;



    // Use this for initialization
    void Awake () {
		currentAction = StayIdle;
		myAnimator = GetComponent<Animator>();
		myRigidbody = GetComponent<Rigidbody2D>();
        //OnFrogDeath += particlesHandler.OnDeath;
       // myCollider = GetComponent<CircleCollider2D>();

    }
	void OnEnable() 
	{
		currentAction = StayIdle;
		state = BeeState.Idle;
        gameObject.layer = groundedLayer; // layer 10 is Hero
        //SetAnimation("Idle");
    }
	
	// Update is called once per frame
	void Update () {
		currentAction();
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.layer == stunCollisionLayer && state == BeeState.Charging) //layer 12 is Obstacle
		{
			StartBeingStunned();
		}
		if (coll.collider.tag == "Hazard")
		{
			Die (coll.collider.name);
		}
        //Debug.Log(coll.collider.name);
	}
	
	private void Die(string causeOfDeath)
	{
        particlesHandler.OnDeath(causeOfDeath);
        if (vectorToPlayer.sqrMagnitude <= (scoreHandler.scoringDistance * scoreHandler.scoringDistance))
        {
            scoreHandler.EnemyDead(this.gameObject, causeOfDeath);
        }
        SetAnimation("Idle");
		WaitUntillAnimatorResets();
        //gameObject.SetActive(false);
	}

    //private void SpawnParticleSystem(GameObject system)
    //{
    //    GameObject newParticleSystem = Instantiate(system, this.transform.position, Quaternion.identity) as GameObject;
    //    Destroy(newParticleSystem, 3f);
    //}
	
	private void StartBeingStunned()
	{
		stateExitTime = Time.timeSinceLevelLoad + stunTime;
		//stunStars.SetActive(true);
		state = BeeState.Stunned;
        gameObject.layer = groundedLayer; // layer 10 is Hero

		SetAnimation("Stunned");

		currentAction = StayStunned;
	}

    //private void SetLayer(String layerName)
    //{
    //    gameObject.layer = LayerMask.NameToLayer(layerName);
    //    myCollider.enabled = false;
    //    myCollider.enabled = true;
    //}
	
	private void StayStunned()
	{
		if(CheckStateExitConditions())
		{
			//stunStars.SetActive(false);
		}
	}

    public void PlayerDetected(GameObject player)
    {
        frog = player;
        StartFollowingPalyer();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //debug.log (other.name);
        if (other.CompareTag("Hazard"))
        {
            Die(other.name);
        }
    }

    private void StartFollowingPalyer()
	{
		state = BeeState.Following;
		currentAction = FollowPlayer;
        gameObject.layer = flyLayer; // layer 18 is Flying
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

	private void WaitUntillAnimatorResets()
	{
		elapsedUpdates = 0;
		currentAction = DisableAfternextUpdate;
	}

	private void DisableAfternextUpdate()
	{
		++elapsedUpdates;
		if (elapsedUpdates == 2) {
			gameObject.SetActive (false);
		}
	}
	
	public void Reset()
	{
		transform.eulerAngles = Vector3.zero;
		currentAction = StayIdle;
		state = BeeState.Idle;
	}
	
}
