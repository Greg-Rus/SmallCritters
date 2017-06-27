using UnityEngine;
using System.Collections;

public class FlyController : MonoBehaviour {
    public Vector2 flyZoneBottomLeft;
    public Vector2 flyZoneTopRight;
    public Vector3 destination = Vector3.zero;
    public float destinationReachedDistance;
    public Rigidbody2D myRigidbody;
    public IDeathReporting deathReport;
    public float reboundForce;
    private Vector3 vectorToDestination;
    private Vector3 heading;
    private BasicMotor motor;
    private bool isAlive = true;

    void Start()
    {
        motor = GetComponent<BasicMotor>();
        deathReport = ServiceLocator.getService<IDeathReporting>();
    }

    void OnEnable()
    {
        isAlive = true;
    }

	void Update ()
    {
        GoToDestination();
    }

    public void SelectDestination()
    {
        destination.x = UnityEngine.Random.Range(flyZoneBottomLeft.x, flyZoneTopRight.x);
        destination.y = UnityEngine.Random.Range(flyZoneBottomLeft.y, flyZoneTopRight.y);
    }
    private void GoToDestination()
    {
        vectorToDestination = destination - this.transform.position;
        heading = vectorToDestination.normalized;
        motor.heading = heading;
        motor.RotateToFaceTarget();
        if (vectorToDestination.sqrMagnitude <= Mathf.Pow(destinationReachedDistance, 2))
        {
            SelectDestination();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Hazard") || collision.collider.CompareTag("Player"))
        {
            Die(collision.collider.name);
        }
        else
        {
            Rebound(collision);
            SelectDestination();
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
        if (isAlive)
        {
            isAlive = false;
            deathReport.EnemyDead(this.gameObject, causeOfDeath);
            this.gameObject.SetActive(false);
        }
        
    }

    private void Rebound(Collision2D collision)
    {
        Vector2 reboundDirection = collision.contacts[0].point - (Vector2)transform.position;
        reboundDirection = reboundDirection.normalized * -1f;
        motor.AddImpulse(reboundDirection * reboundForce);
    }
}
