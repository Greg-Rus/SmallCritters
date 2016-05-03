using UnityEngine;
using System.Collections;
using System;

public enum FireBeetleState { Idle, Following, Attacking};
public class FireBeetleController : MonoBehaviour, IPlayerDetection
{
    public Animator myAnimator;
    public Rigidbody2D myRigidbody;
    public ScoreHandler scoreHandler;
    public FireBeetleState state;
    public float speed;
    public float attackDistance;
    public GameObject fireBall;
    public Transform firingPoint;
    public float fireBallSpeed = 3f;
   
    GameObject frog;
    Action currentAction;
    Vector3 vectorToPlayer;
    Vector3 heading;
    Vector3 target;
    int elapsedUpdates;
    string currentAnimation;

    // Use this for initialization
    void Start()
    {
        state = FireBeetleState.Idle;
        currentAction = StayIdle;
    }

    // Update is called once per frame
    void Update()
    {
        currentAction();
    }

    public void AttackComplete()
    {
        //StartFollowingPlayer();
    }
    public void DeployProjectile()
    {
        GameObject newFireBall = Instantiate(fireBall, firingPoint.position, Quaternion.identity) as GameObject;
        FireBallController newBallController = newFireBall.GetComponent<FireBallController>();
        Vector3 heading = (target - firingPoint.transform.position).normalized;
        newBallController.Target(firingPoint, heading, OnFireBallHit);
        newBallController.myRigidBody.AddForce(heading * fireBallSpeed, ForceMode2D.Impulse);

    }

    private void OnFireBallHit()
    {
        StartFollowingPlayer();
    }

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
        scoreHandler.EnemyDead(this.gameObject, causeOfDeath);
        Destroy(this.gameObject);
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
        myRigidbody.AddForce(heading * speed);
        myAnimator.SetFloat("Speed", myRigidbody.velocity.magnitude);
        if (vectorToPlayer.sqrMagnitude <= Mathf.Pow(attackDistance, 2))
        {
            StartAttackingPlayer();
        }
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

    }

    public void UpdatePlayerLocation()
    {
        vectorToPlayer = frog.transform.position - this.transform.position;
        heading = vectorToPlayer.normalized;
    }

    private void RotateToFacePlayer()
    {
        float angle = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
        //angle += 90f;
        myRigidbody.MoveRotation(angle);
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
