using UnityEngine;
using System.Collections;
using System;

public class FireBeetleController : MonoBehaviour, IPlayerDetection
{
    public Animator myAnimator;
    public Rigidbody2D myRigidbody;
    public IDeathReporting deathReport;
    public float maxProjectileRange = 5f;
    public GameObject fireBall;
    public Transform firingPoint;
    public float fireBallSpeed = 3f;
    public Transform myTransform;
    public float angleToPlayerDelta;
    public DeathParticleSystemHandler deathParticles;
	public ParticleSystem attackCharge;
    public float angleToPlayer;

    private GameObject frog;
    private Vector3 vectorToPlayer;
    private Vector3 heading;
    private bool alive = true;
    private IAudio myAudio;
    public FireBeetleData data;
    private FireBeetleFSM FSM;
    private BasicMotor motor;
    private float shotCooldownTimeout = 0f;
    private SpriteRenderer myRenderer;


    void Awake()
    {
        FSM = new FireBeetleFSM(this);
        motor = GetComponent<BasicMotor>();
        myRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        deathReport = ServiceLocator.getService<IDeathReporting>();
        myAudio = ServiceLocator.getService<IAudio>();
    }
    void OnEnable()
    {
        frog = null;
        alive = true;
        FSM.Reset();
        motor.enabled = false;
		attackCharge.Clear();
		attackCharge.Stop();
        myAnimator.Rebind();
	}
    void Update()
    {
        FSM.CurrentAction();
    }

    public void DeployProjectile()
    {
		attackCharge.Clear();
		attackCharge.Stop();
		GameObject newFireBall = Instantiate(fireBall, firingPoint.position, Quaternion.identity) as GameObject;
        FireBallController newBallController = newFireBall.GetComponent<FireBallController>();
        Vector3 heading = (frog.transform.position - firingPoint.transform.position).normalized;
        newBallController.Aim(firingPoint, maxProjectileRange);
        newBallController.myRigidBody.AddForce(heading * fireBallSpeed, ForceMode2D.Impulse);
        myAudio.PlaySound(Sound.BeatleSpit);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Hazard"))
        {
            Die(coll.collider.name);
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
        if (alive)
        {
            alive = false;
            if(myRenderer.isVisible) deathReport.EnemyDead(this.gameObject, causeOfDeath);
            deathParticles.OnDeath(causeOfDeath);
            SetAnimation("Idle");
            WaitUntillAnimatorResets();
            myAudio.PlayEnemyDeathSound(causeOfDeath);
        }
    }

    public void PlayerDetected(GameObject player)
    {
        if (frog == null)
        {
            frog = player;
            FSM.StartFollowingPlayer();
            if (motor.enabled == false)
            {
                motor.enabled = true;
            }
        }
    }

    public void UpdatePlayerLocation()
    {
        vectorToPlayer = frog.transform.position - myTransform.position;
        heading = vectorToPlayer.normalized;
    }

    public void RotateToFacePlayer()
    {
        motor.heading = heading;
        motor.SmoothRotateToFaceHeading();
    }

    private void SetAnimation(string stringInput)
    {
        myAnimator.SetTrigger(stringInput);
    }
    private void WaitUntillAnimatorResets()
    {
        StartCoroutine(DisableAfterNextUpdate());
    }

    private IEnumerator DisableAfterNextUpdate()
    {
        yield return null;
        gameObject.SetActive(false);
    }

    public void SetSpeed(float speed)
    {
        motor.heading = heading;
        motor.speed = speed;
    }

    public void Attack()
    {
        SetAnimation("Attack");
        myAnimator.SetFloat("Speed", 0f);
        motor.speed = 0f;
		attackCharge.Play();
    }

    public void AttackComplete()
    {
		shotCooldownTimeout = Time.timeSinceLevelLoad + data.shotCooldownTime;
        FSM.StartFollowingPlayer();
    }

    public void UpdateAnimationSpeed()
    {
        if (motor.IsMovingForward())
        {
            myAnimator.SetFloat("Speed", motor.GetVelocityMagnitude());
        }
        else
        {
            myAnimator.SetFloat("Speed", -motor.GetVelocityMagnitude());
        }
    }

    public bool IsInRange(float range)
    {
        return (vectorToPlayer.sqrMagnitude <= Mathf.Pow(range, 2f)) ? true : false;
    }
    public bool IsRotationWithinError()
    {
        return (motor.angleToTargetDelta <= motor.minRotationError) ? true : false;
    }
    public bool IsReadyToFire()
    {
        return (shotCooldownTimeout <= Time.timeSinceLevelLoad) ? true : false;
    }
}
