using UnityEngine;
using System.Collections;
using System;

public class BeeController : MonoBehaviour {
	private Animator myAnimator;
    private GameObject frog;
    private Vector3 vectorToPlayer;
    private Vector3 heading;
    private BasicMotor motor;
    private BeeFSM myFSM;
    private bool alive = false;
    private string currentAnimation;
    private IAudio audio;

    public BeeData data = new BeeData();
    public int flyLayer;
    public int groundedLayer;
    public int stunCollisionLayer;
    public IDeathReporting deathReport;
    public DeathParticleSystemHandler particlesHandler;

    void Awake ()
    {
        myAnimator = GetComponent<Animator>();
        motor = GetComponent<BasicMotor>();
        myFSM = new BeeFSM(this);
    }
    void Start()
    {
        deathReport = ServiceLocator.getService<IDeathReporting>();
        audio = ServiceLocator.getService<IAudio>();
    }
	void OnEnable() 
	{
        alive = true;
        frog = null;
        MakeBeeGrounded();
        myFSM.Reset();
        motor.speed = 0;
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
            audio.PlayEnemyDeathSound(causeOfDeath);
        }
	}

    public void PlayerDetected(GameObject player)
    {
        if (frog == null)
        {
            frog = player;
            myFSM.OnPlayerDetected();
        }
    }

	public void UpdatePlayerLocation()
	{
		vectorToPlayer = frog.transform.position - this.transform.position;
		heading = vectorToPlayer.normalized;
        motor.heading = heading;
	}
	
	public void RotateToFacePlayer()
	{
        motor.RotateToFaceTarget();
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
        motor.speed = data.flySpeed;
    }
    public void ApplyChargingForce()
    {
        motor.speed = data.chargeSpeed;
    }
    public void RapidStop()
    {
        motor.RapidStop();
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
