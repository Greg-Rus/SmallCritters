using UnityEngine;
using System.Collections;
using System;

public class FireBeetleController : MonoBehaviour, IPlayerDetection
{
    public Animator myAnimator;
    public Rigidbody2D myRigidbody;
    public IDeathReporting deathReport;
    public float maxProjectileRange = 5f;
    public float minRotationError; //error in degrees where it is still acceptable to fire a projectile.
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

    void Awake()
    {
        FSM = new FireBeetleFSM(this);
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
    }
    void Update()
    {
        FSM.CurrentAction();
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
    }

    public void UpdatePlayerLocation()
    {
        vectorToPlayer = frog.transform.position - myTransform.position;
        heading = vectorToPlayer.normalized;
    }

    public void RotateToFacePlayer()
    {
        angleToPlayer = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
        if (angleToPlayer < 0)
        {
            angleToPlayer += 360f;
        }
        angleToPlayerDelta = Math.Abs(myTransform.eulerAngles.z - angleToPlayer);
        if (angleToPlayerDelta >= minRotationError)
        {
            float smoothAngle = Mathf.MoveTowardsAngle(myTransform.rotation.eulerAngles.z, angleToPlayer, data.rotationSpeed);
            myRigidbody.MoveRotation(smoothAngle);
        }
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
        myRigidbody.AddForce(heading * speed);
        if (Vector3.Dot(myRigidbody.velocity.normalized, myTransform.forward) > 1f)
        {
            myAnimator.SetFloat("Speed", myRigidbody.velocity.magnitude);
        }
        else
        {
            myAnimator.SetFloat("Speed", -myRigidbody.velocity.magnitude);
        }
    }

    public void Attack()
    {
        SetAnimation("Attack");
        myAnimator.SetFloat("Speed", 0f);
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
        return (angleToPlayerDelta <= minRotationError) ? true : false;
    }
    public bool IsReadyToFire()
    {
        return (lastShotTime + data.shotCooldownTime <= Time.timeSinceLevelLoad) ? true : false;
    }

}
