using UnityEngine;
using System.Collections;
using System;

public class FireBallController : MonoBehaviour {
    public Rigidbody2D myRigidBody;
    public Transform beetle;
    public Vector3 target;
    public Renderer myRenderer;
    public Vector3 heading;
    public float speed = 2f;
    public Action OnHit;
    public float maxRange = 5f;
    private Vector3 startPosition;

    void Start()
    {
        gameObject.name = "FlameBall";
    }

    void Update()
    {
        ChackIfAtMaxRnage();
    }

    public void Target(Transform beetlePosition, Vector3 heading, Action OnHit)
    {
        this.beetle = beetlePosition;
        startPosition = beetle.position;
        this.OnHit = OnHit;
        this.heading = heading;
    }

    public void Aim(Transform beetlePosition, float maxRange)
    {
        this.beetle = beetlePosition;
        startPosition = beetle.position;
        this.maxRange = maxRange;
    }

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Hazard")) Explode();
    }

    private void Explode()
    {
        Destroy(this.gameObject);
    }
}
