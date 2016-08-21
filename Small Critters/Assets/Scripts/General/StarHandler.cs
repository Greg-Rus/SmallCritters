using UnityEngine;
using System.Collections;
using System;

public class StarHandler : MonoBehaviour {
    public Transform frog;
    public float moveSpeed;
    public float turnSpeed;
    public float decaySpeed;
    public float inflateSpeed;
    public float decayScaleThreshold;
    public float normalScale = 1f;
    public float initialScale = 0.1f;
    public int points;
    public float scoringDistance;
    public float pickUpDistance = 0.1f;
    public ScoreStarState state;
    public CircleCollider2D playerDetectionCircle;

    private Action<int> OnStarPickup;
    private IAudio myAudio;
    Vector3 vectorToFrog;
    float scale;



    void Start () {
        transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360));
        myAudio = ServiceLocator.getService<IAudio>();
    }

    public void Configure(int points, float scoringDistance, Action<int> OnStarPickup  )
    {
        vectorToFrog = Vector3.zero;
        transform.localScale = Vector3.one * initialScale;
        this.points = points;
        playerDetectionCircle.radius = scoringDistance;
        this.OnStarPickup = OnStarPickup;
    }
	
	void Update ()
    {
        switch (state)
        {
            case ScoreStarState.Starting: Inflate(); break;
            case ScoreStarState.Waiting: Decay(); break;
            case ScoreStarState.Following: MoveToFrog(); CheckForFrogProximity(); break;
            case ScoreStarState.BeingPickedUp: MoveToFrog(); Deflate(); break;
        }
    }

    void Inflate()
    {
        ChangeScale(inflateSpeed);
        if (scale >= normalScale)
        {
            transform.localScale = Vector3.one * normalScale;
            scale = normalScale;
            state = ScoreStarState.Waiting;
        }
    }
    void Decay()
    {
        if (frog != null)
        {
            state = ScoreStarState.Following;
        }
        else
        {
            ChangeScale(-decaySpeed);
            if (scale <= decayScaleThreshold)
            {
                Destroy(this.gameObject);
            }
        }
        
    }

    void MoveToFrog()
    {
        vectorToFrog = frog.position - transform.position;
        float angle = Mathf.Atan2(vectorToFrog.y, vectorToFrog.x) * Mathf.Rad2Deg;
        float newAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angle, turnSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, 0, newAngle);
        transform.position = transform.position + transform.right * Time.deltaTime * moveSpeed;

    }

    void CheckForFrogProximity()
    {
        if (vectorToFrog.sqrMagnitude <= pickUpDistance)
        {
            state = ScoreStarState.BeingPickedUp;
        }
    }

    void Deflate()
    {
        ChangeScale(-inflateSpeed);
        if (transform.localScale.x <= initialScale)
        {
            OnStarPickup(points);
            myAudio.PlaySound(Sound.StarPickup);
            Destroy(this.gameObject);
        }
       
    }

    void ChangeScale(float speed)
    {
        scale += speed * Time.deltaTime;
        transform.localScale = Vector3.one * scale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        frog = other.transform;
    }
}
