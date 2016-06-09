﻿using UnityEngine;
using System.Collections;
using System;

public enum FireBeetleState { Idle, Following, Attacking};
public class FireBeetleController : MonoBehaviour, IPlayerDetection
{
    public Animator myAnimator;
    public Rigidbody2D myRigidbody;
    public ScoreHandler scoreHandler;
    public FireBeetleState state;
    public float walkSpeed;
    public float rotationSpeed;
    public float attackDistanceMax;
    public float attackDistanceMin;
    public float minRotationError; //error in degrees where it is still acceptable to fire a projectile.
    public GameObject fireBall;
    public Transform firingPoint;
    public float fireBallSpeed = 3f;
    public Transform myTransform;
    public float angleToPlayerDelta;
    public DeathParticleSystemHandler deathParticles;
    public float shotCooldownTime;
    private float lastShotTime = 0;



    GameObject frog;
    Action currentAction;
    Vector3 vectorToPlayer;
    public float angleToPlayer;
    Vector3 heading;
    Vector3 target;
    int elapsedUpdates;
    string currentAnimation;
    bool alive = true;

    // Use this for initialization
    void Start()
    {
        state = FireBeetleState.Idle;
        currentAction = StayIdle;
        //myTransform = GetComponentInParent<Transform>();
    }
    void OnEnable()
    {
        state = FireBeetleState.Idle;
        currentAction = StayIdle;
        frog = null;
        alive = true;
        myTransform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        currentAction();
    }

    public void AttackComplete()
    {
        lastShotTime = Time.timeSinceLevelLoad;
        StartFollowingPlayer();
    }


    public void DeployProjectile()
    {
        GameObject newFireBall = Instantiate(fireBall, firingPoint.position, Quaternion.identity) as GameObject;
        FireBallController newBallController = newFireBall.GetComponent<FireBallController>();
        Vector3 heading = (frog.transform.position - firingPoint.transform.position).normalized;

        //newBallController.Target(firingPoint, heading, OnFireBallHit);
        newBallController.Aim(firingPoint, 5f); //TODO Decide max range in DifficultyManager;
        newBallController.myRigidBody.AddForce(heading * fireBallSpeed, ForceMode2D.Impulse);

    }

    //private void OnFireBallHit()
    //{
    //    StartFollowingPlayer();
    //}

    void OnCollisionEnter2D(Collision2D coll)
    {
        //if (coll.gameObject.layer == stunCollisionLayer && state == BeeState.Charging) //layer 12 is Obstacle
        //{
        //    StartBeingStunned();
        //}
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
            scoreHandler.EnemyDead(this.gameObject, causeOfDeath);
            deathParticles.OnDeath(causeOfDeath);
            WaitUntillAnimatorResets();
            //this.gameObject.SetActive(false);
        }
    }

    public void PlayerDetected(GameObject player)
    {
        frog = player;
        StartFollowingPlayer();
    }

    private void StayIdle()
    {
        //Dummy state;
    }

    public void StartFollowingPlayer()
    {
        state = FireBeetleState.Following;
        //SetAnimation("FollowPlayer");
        currentAction = FollowPlayer;

    }

    private void FollowPlayer()
    {
        UpdatePlayerLocation();
        RotateToFacePlayer();
        if (vectorToPlayer.sqrMagnitude < Mathf.Pow(attackDistanceMin, 2))
        {
            StartEvadingPlayer();
        }
        else if (vectorToPlayer.sqrMagnitude <= Mathf.Pow(attackDistanceMax, 2) && 
            angleToPlayerDelta <= minRotationError &&
            lastShotTime + shotCooldownTime <= Time.timeSinceLevelLoad)
        {
            StartAttackingPlayer();
        }
        myRigidbody.AddForce(heading * walkSpeed);
        myAnimator.SetFloat("Speed", myRigidbody.velocity.magnitude);

    }

    private void StartEvadingPlayer()
    {
        currentAction = EvadePlayer;
    }

    private void EvadePlayer()
    {
        UpdatePlayerLocation();
        RotateToFacePlayer();
        if (vectorToPlayer.sqrMagnitude >= Mathf.Pow(attackDistanceMax, 2))
        {
            StartFollowingPlayer();
        }
        else if (vectorToPlayer.sqrMagnitude <= Mathf.Pow(attackDistanceMax, 2) &&
            angleToPlayerDelta <= minRotationError &&
            lastShotTime + shotCooldownTime <= Time.timeSinceLevelLoad)
        {
            StartAttackingPlayer();
        }
        myRigidbody.AddForce(heading * -walkSpeed);
        myAnimator.SetFloat("Speed", -myRigidbody.velocity.magnitude);
    }

    private void StartAttackingPlayer()
    {
        state = FireBeetleState.Attacking;
        SetAnimation("Attack");
        currentAction = Attack;
        myAnimator.SetFloat("Speed", 0f);
        target = frog.transform.position;
    }

    private void Attack()
    {
        UpdatePlayerLocation();
        RotateToFacePlayer();
    }

    public void UpdatePlayerLocation()
    {
        vectorToPlayer = frog.transform.position - myTransform.position;
        heading = vectorToPlayer.normalized;
    }

    private void RotateToFacePlayer()
    {
        angleToPlayer = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
        if (angleToPlayer < 0)
        {
            angleToPlayer += 360f;
        }
        angleToPlayerDelta = Math.Abs(myTransform.eulerAngles.z - angleToPlayer);
        if (angleToPlayerDelta >= minRotationError)
        {
            float smoothAngle = Mathf.MoveTowardsAngle(myTransform.rotation.eulerAngles.z, angleToPlayer, rotationSpeed);
            myRigidbody.MoveRotation(smoothAngle);
        }
    }

    private void SetAnimation(string stringInput)
    {
        //Animator anim =transform.GetComponent<Animator>();
        //Debug.Log("Setting Anim to : " + stringInput);
        myAnimator.SetTrigger(stringInput);
        //if (currentAnimation == stringInput)
        //{
        //}
        //else {
        //    if (currentAnimation != null)
        //    {
        //        myAnimator.ResetTrigger(currentAnimation);
        //    }
        //    myAnimator.SetTrigger(stringInput);
        //    currentAnimation = stringInput;
        //}
    }
    private void WaitUntillAnimatorResets()
    {
        SetAnimation("Reset");
        elapsedUpdates = 0;
        currentAction = DisableAfterNextUpdate;
    }

    private void DisableAfterNextUpdate()
    {
        ++elapsedUpdates;
        if (elapsedUpdates == 2)
        {
            gameObject.SetActive(false);
        }
    }

}
