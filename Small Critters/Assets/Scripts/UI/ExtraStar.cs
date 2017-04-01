using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExtraStar : MonoBehaviour {
    public Vector3 rotateAngle;
    public float moveSpeed;
    public float rightWall;
    public float bottom;
    public float resizeTime;

    private Action state;
    public RectTransform myRect;
    private float timer = 0f;
	// Use this for initialization
	void Start ()
    {
        myRect = GetComponent<RectTransform>();
        myRect.localScale = Vector3.zero;
        state = Grow;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Rotate();
        state();
    }

    private void Rotate()
    {
        myRect.Rotate(rotateAngle);
    }

    private void Grow()
    {
        timer += Time.deltaTime;
        float newScale = timer / resizeTime;
        this.transform.localScale = Vector3.one * newScale;
        if (timer >= resizeTime) state = MoveToWall;
    }

    private void MoveToWall()
    {
        Vector3 newPosition = myRect.localPosition;
        newPosition.x += moveSpeed * Time.deltaTime;
        myRect.localPosition = newPosition;

        if (myRect.position.x >= rightWall) state = MoveDown;
    }

    private void MoveDown()
    {
        Vector3 newPosition = myRect.localPosition;
        newPosition.y -= moveSpeed * Time.deltaTime;
        myRect.localPosition = newPosition;

        if (myRect.position.y <= bottom) state = Shrink;
    }



    private void Shrink()
    {
        timer -= Time.deltaTime;
        float newScale = timer / resizeTime;
        this.transform.localScale = Vector3.one * newScale;

        if (timer <= 0f) Destroy(gameObject);
    }
}
