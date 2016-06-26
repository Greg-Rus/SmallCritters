using UnityEngine;
using System.Collections;
using System;

public class BeeController : MonoBehaviour {
	Animator myAnimator;
	Rigidbody2D myRigidbody;
	GameObject frog;
	Vector3 vectorToPlayer;
	Vector3 heading;
    public int flyLayer;
    public int groundedLayer;
    public int stunCollisionLayer;
    
	private string currentAnimation;
    public IDeathReporting deathReport;
    public DeathParticleSystemHandler particlesHandler;
    private bool alive = false;
    private BeeFSM myFSM;
    public BeeData data = new BeeData();

    void Awake ()
    {
        myAnimator = GetComponent<Animator>();
		myRigidbody = GetComponent<Rigidbody2D>();
        myFSM = new BeeFSM(this);
    }
    void Start()
    {
        deathReport = ServiceLocator.getService<IDeathReporting>();
    }
	void OnEnable() 
	{
        alive = true;
        MakeBeeGrounded();
        myFSM.Reset();
    }
    void Update()
    {
        myFSM.CurrentAction();
    }
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.layer == stunCollisionLayer && data.state == BeeState.Charging)
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
        myRigidbody.AddForce(heading * data.flySpeed);
    }
    public void ApplyChargingForce()
    {
        myRigidbody.AddForce(heading * data.chargeSpeed);
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
