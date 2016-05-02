using UnityEngine;
using System.Collections;

public class FlyController : MonoBehaviour {
    public Vector2 flyZoneBottomLeft;
    public Vector2 flyZoneTopRight;
    public Vector3 destination = Vector3.zero;
    public float speed;
    public float destinationReachedDistance;
    public Rigidbody2D myRigidbody;
    public ScoreHandler scoreHandler;
    private Vector3 vectorToDestination;
    private Vector3 heading;

    // Use this for initialization
    void Start ()
    {
        SelectDestination();
    }
	
	// Update is called once per frame
	void Update ()
    {
        GoToDestination();
    }

    private void SelectDestination()
    {
        destination.x = RandomLogger.GetRandomRange(this, flyZoneBottomLeft.x, flyZoneTopRight.x);
        destination.y = RandomLogger.GetRandomRange(this, flyZoneBottomLeft.y, flyZoneTopRight.y);
    }
    private void GoToDestination()
    {
        vectorToDestination = destination - this.transform.position;
        heading = vectorToDestination.normalized;
        RotateToDestination();
        myRigidbody.AddForce(heading * speed);
        if (vectorToDestination.sqrMagnitude <= Mathf.Pow(destinationReachedDistance, 2))
        {
            SelectDestination();
        }
    }

    private void RotateToDestination()
    {
        float angle = Mathf.Atan2(vectorToDestination.y, vectorToDestination.x) * Mathf.Rad2Deg;
        myRigidbody.MoveRotation(angle);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Hazard") || coll.collider.CompareTag("Player"))
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
}
