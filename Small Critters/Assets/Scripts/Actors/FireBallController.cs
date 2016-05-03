using UnityEngine;
using System.Collections;
using System;

public class FireBallController : MonoBehaviour {
    //public FireBallController myBeetleController;
    public Rigidbody2D myRigidBody;
    public Transform beetle;
    public Vector3 target;
    public Renderer myRenderer;
    public Vector3 heading;
    public float speed = 2f;
    public Action OnHit;
    public float maxRange = 5f;
    private Vector3 startPosition;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //MoveToTarget();
        //velocity = myRigidBody.velocity.magnitude;


    }
    void Update()
    {
        ChackIfAtMaxRnage();
        //if (myRigidBody.velocity.magnitude == 0f)
        //{
        //    myRigidBody.AddForce(heading * speed, ForceMode2D.Impulse);
        //}
    }

    public void Target(Transform beetlePosition, Vector3 heading, Action OnHit)
    {
        //myRigidBody.MovePosition(beetlePosition.position);
        this.beetle = beetlePosition;
        startPosition = beetle.position;
        //this.target = target;
        this.OnHit = OnHit;
        this.heading = heading;//(target - startPosition).normalized;
        //Debug.DrawLine(beetle.position, target, Color.red, 5f);
        //myRigidBody.AddForce(heading * speed, ForceMode2D.Impulse);
    }

    //private void MoveToTarget()
    //{
    //    //myRigidBody.AddForce(heading * speed);
    //}
    private void ChackIfAtMaxRnage()
    {
        float distanceTraveledSqr = (this.transform.position - startPosition).sqrMagnitude;
        if (distanceTraveledSqr >= Mathf.Pow(maxRange, 2))
        {
            Explode();
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Explode();
    }

    private void Explode()
    {
        if (beetle != null)
        {
            OnHit();
        }

        Destroy(this.gameObject);
    }

}
