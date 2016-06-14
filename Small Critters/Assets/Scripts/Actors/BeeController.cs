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
	//float stateExitTime;
	public float chargeTime;
	public float flySpeed;
	public float chargeSpeed;
	public float chargeDistance;
    public int flyLayer;
    public int groundedLayer;
    public int stunCollisionLayer;

	public float stunTime;
	//public BeeState state;
	//public float chaseTimeLeft;
	private string currentAnimation;
    public IDeathReporting deathReport;
    public DeathParticleSystemHandler particlesHandler;
    private bool alive = false;
    private BeeFSM myFSM;

    void Awake () {
		myAnimator = GetComponent<Animator>();
		myRigidbody = GetComponent<Rigidbody2D>();
        myFSM = GetComponent<BeeFSM>();
    }

    void Start()
    {
        deathReport = ServiceLocator.getService<IDeathReporting>();
    }
	void OnEnable() 
	{
        alive = true;
        MakeBeeGrounded();
    }
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.layer == stunCollisionLayer && myFSM.state == BeeState.Charging)
		{
            myFSM.OnColision();
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
            deathReport.EnemyDead(this.gameObject, causeOfDeath);
            SetAnimation("Idle");
            WaitUntillAnimatorResets();
        }
	}

    public void PlayerDetected(GameObject player)
    {
        frog = player;
        myFSM.OnPlayerDetected();
    }

	public void UpdatePlayerLocation()
	{
		vectorToPlayer = frog.transform.position - this.transform.position;
		heading = vectorToPlayer.normalized;
	}
	
	public void RotateToFacePlayer()
	{
		float angle = Mathf.Atan2(heading.y,heading.x) * Mathf.Rad2Deg;
		myRigidbody.MoveRotation(angle);
	}
	
	public void SetAnimation(string stringInput){
		if(currentAnimation != stringInput)
        {
			if(currentAnimation!=null)
            {
				myAnimator.ResetTrigger(currentAnimation);
			}
			myAnimator.SetTrigger(stringInput);
			currentAnimation=stringInput;
		}
	}

	private void WaitUntillAnimatorResets()
	{
		StartCoroutine(DisableAfterNextUpdate());
	}

	private IEnumerator DisableAfterNextUpdate()
	{
        yield return null;
        gameObject.SetActive (false);
	}
	
    public void ApplyFlyingForce()
    {
        myRigidbody.AddForce(heading * flySpeed);
    }
    public void ApplyChargingForce()
    {
        myRigidbody.AddForce(heading * flySpeed);
    }
    public void RapidStop()
    {
        myRigidbody.velocity = Vector3.zero;
    }

    public void MakeBeeGrounded()
    {
        gameObject.layer = groundedLayer;
    }

    public void MakeBeeAirborn()
    {
        gameObject.layer = flyLayer;
    }

    public bool CheckIfInRange(float range)
    {
        return (vectorToPlayer.sqrMagnitude <= Mathf.Pow(range, 2f)) ? true : false;
    }
	
}
