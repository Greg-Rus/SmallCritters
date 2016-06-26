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
    public float angleToPlayer;

    private float lastShotTime = 0;
    private GameObject frog;
    private Vector3 vectorToPlayer;
    private Vector3 heading;
    private bool alive = true;
    public FireBeetleData data;
    private FireBeetleFSM FSM;
    private BasicMotor motor;

    void Awake()
    {
        FSM = new FireBeetleFSM(this);
        motor = GetComponent<BasicMotor>();
    }

    void Start()
    {
        deathReport = ServiceLocator.getService<IDeathReporting>();
    }
    void OnEnable()
    {
        frog = null;
        alive = true;
        FSM.Reset();
        motor.enabled = false;
    }
    void Update()
    {
        FSM.CurrentAction();
        if (data.state == FireBeetleState.Attacking && motor.speed != 0f)
        {
            Debug.Log("motor running while attacking");
        }
    }

    public void DeployProjectile()
    {
        GameObject newFireBall = Instantiate(fireBall, firingPoint.position, Quaternion.identity) as GameObject;
        FireBallController newBallController = newFireBall.GetComponent<FireBallController>();
        Vector3 heading = (frog.transform.position - firingPoint.transform.position).normalized;
        newBallController.Aim(firingPoint, maxProjectileRange); //TODO Decide max range in DifficultyManager;
        newBallController.myRigidBody.AddForce(heading * fireBallSpeed, ForceMode2D.Impulse);
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
            deathReport.EnemyDead(this.gameObject, causeOfDeath);
            deathParticles.OnDeath(causeOfDeath);
            WaitUntillAnimatorResets();
        }
    }

    public void PlayerDetected(GameObject player)
    {
        frog = player;
        FSM.StartFollowingPlayer();
        if (motor.enabled == false)
        {
            motor.enabled = true;
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

    public void Move(float speed)
    {
        motor.heading = heading;
        motor.speed = speed;
        if (data.state == FireBeetleState.Attacking)
        {
            Debug.Log("Move used while attacking");
        }
        if (motor.IsMovingForward())
        {
            myAnimator.SetFloat("Speed", motor.GetVelocityMagnitude());
        }
        else
        {
            myAnimator.SetFloat("Speed", -motor.GetVelocityMagnitude());
        }
    }

    public void Attack()
    {
        SetAnimation("Attack");
        myAnimator.SetFloat("Speed", 0f);
        motor.speed = 0f;
    }

    public void AttackComplete()
    {
        lastShotTime = Time.timeSinceLevelLoad;
        FSM.StartFollowingPlayer();
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
        return (lastShotTime + data.shotCooldownTime <= Time.timeSinceLevelLoad) ? true : false;
    }

}
