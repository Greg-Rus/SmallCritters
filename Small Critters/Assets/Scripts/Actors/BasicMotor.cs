using UnityEngine;
using System.Collections;
using System;

public class BasicMotor : MonoBehaviour {
    Rigidbody2D myRigidbody;
    Transform myTransform;
    FireBeetleController myController;
    public float angleToTargetDelta;
    public Vector2 heading;
    public float speed;
    public float minRotationError;

    // Use this for initialization
    void Awake ()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
        myController = GetComponent<FireBeetleController>();
    }
	
	void FixedUpdate ()
    {
        Move();
    }

    public void Move()
    {
        myRigidbody.AddForce(heading * speed);
    }

    public void RotateToFaceTarget()
    {
        float angle = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
        myRigidbody.MoveRotation(angle);
    }

    public void SmoothRotateToFaceHeading()
    {
        float angleToTarget = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
        if (angleToTarget < 0)
        {
            angleToTarget += 360f;
        }
        angleToTargetDelta = Math.Abs(myTransform.eulerAngles.z - angleToTarget);
        if (angleToTargetDelta >= minRotationError)
        {
            float smoothAngle = Mathf.MoveTowardsAngle(myTransform.rotation.eulerAngles.z, angleToTarget, myController.data.rotationSpeed);
            myRigidbody.MoveRotation(smoothAngle);
        }
    }

    public bool IsMovingForward()
    {
        return (Vector3.Dot(myRigidbody.velocity.normalized, myTransform.forward) > 1f) ? true : false;
    }

    public float GetVelocityMagnitude()
    {
        return myRigidbody.velocity.magnitude;
    }

    public void RapidStop()
    {
        myRigidbody.velocity = Vector3.zero;
        speed = 0f;
    }

    public void AddImpulse(Vector2 force)
    {
        myRigidbody.AddForce(force, ForceMode2D.Impulse);
    }
}
