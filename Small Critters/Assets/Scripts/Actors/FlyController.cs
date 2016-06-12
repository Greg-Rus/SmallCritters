using UnityEngine;
using System.Collections;

public class FlyController : MonoBehaviour {
    public Vector2 flyZoneBottomLeft;
    public Vector2 flyZoneTopRight;
    public Vector3 destination = Vector3.zero;
    public float speed;
    public float destinationReachedDistance;
    public Rigidbody2D myRigidbody;
    public IDeathReporting deathReport;
    private Vector3 vectorToDestination;
    private Vector3 heading;

    void Start()
    {
        deathReport = ServiceLocator.getService<IDeathReporting>();
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
        SelectDestination();
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
        deathReport.EnemyDead(this.gameObject, causeOfDeath);
        this.gameObject.SetActive(false);
    }
}
