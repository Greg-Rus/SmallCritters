using UnityEngine;
using System.Collections;
using System;

public class StarHandler : MonoBehaviour {
    public Transform frog;
    public float moveSpeed;
    public float turnSpeed;
    public float decaySpeed;
    public float decayScaleThreshold;
    public int points;
    public float scoringDistance;
    public ParticleSystem sparcleTrail;
    public CircleCollider2D playerDetectionCircle;

    private Action<int> OnStarPickup;
    private float scale = 1;
    private IAudio myAudio;

    void Start () {
        transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360));
        myAudio = ServiceLocator.getService<IAudio>();
    }

    public void Configure(int points, float scoringDistance, Action<int> OnStarPickup  )
    {
        this.points = points;
        playerDetectionCircle.radius = scoringDistance;
        this.OnStarPickup = OnStarPickup;
    }
	
	void Update ()
    {
        if (frog == null)
        {
            Decay();
        }
        else
        {
            MoveToFrog();
        }
    }
    void Decay()
    {
        scale -= decaySpeed * Time.deltaTime;
        transform.localScale = Vector3.one * scale;
        if (transform.localScale.x <= decayScaleThreshold)
        {
            Destroy(this.gameObject);
        }
    }

    void MoveToFrog()
    {
        Vector3 relativePos = frog.position - transform.position;
        float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        float newAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angle, turnSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, 0, newAngle);
        transform.position = transform.position + transform.right * Time.deltaTime * moveSpeed;
        if (relativePos.sqrMagnitude <= 0.2f)
        {
            OnStarPickup(points);
            myAudio.PlaySound(Sound.StarPickup);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        frog = other.transform;
        sparcleTrail.Play();
    }
}
