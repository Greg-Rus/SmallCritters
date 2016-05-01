﻿using UnityEngine;
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
    private float scale = 1;
    public ParticleSystem sparcleTrail;
    public ScoreHandler scoreHandler;
    public CircleCollider2D playerDetectionCircle;
    private Action<int> OnStarPickup;
	// Use this for initialization
	void Start () {
        transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360));
        //playerDetectionCircle = GetComponent<CircleCollider2D>();
    }

    public void Configure(int points, float scoringDistance, Action<int> OnStarPickup  )
    {
        this.points = points;
        playerDetectionCircle.radius = scoringDistance;
        this.OnStarPickup = OnStarPickup;
    }
	
	// Update is called once per frame
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
        //transform.Rotate(transform.forward, newAngle);
        transform.position = transform.position + transform.right * Time.deltaTime * moveSpeed;
        if (relativePos.sqrMagnitude <= 0.2f)
        {
            //scoreHandler.StarCollected();
            OnStarPickup(points);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        frog = other.transform;
        sparcleTrail.Play();
    }
}
