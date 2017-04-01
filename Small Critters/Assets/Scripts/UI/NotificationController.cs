using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour {
    public float target = 0f;
    public float dropSpeed = 200f;
    public float waitTime = 1f;
    public float shrinkTime = 1f;
    private RectTransform myRect;
    private Action callBack;
    private float timer;
    private Text myText;

	void Start ()
    {
        myRect = GetComponent<RectTransform>();
        myText = GetComponent<Text>();
    }

    // Update is called once per frame
    public void SetNotificationText(string text)
    {
        myText.text = text;
    }


    public void MoveDown(float target)
    {
        this.target = target;
        StopCoroutine("MovingDown");
        StartCoroutine(MovingDown());
    }

    public void StartUp(Action callBack)
    {
        this.callBack = callBack;
        StartCoroutine(Wait());
    }

    public void Reset()
    {
        myText.text = "";
        StopAllCoroutines();
        myRect.localPosition = Vector3.zero;
        this.transform.localScale = Vector3.one;
    }

    IEnumerator Wait()
    {
        float timer = waitTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Shrink());
    }

    IEnumerator Shrink()
    {
        timer = shrinkTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float newScale = timer / shrinkTime;
            this.transform.localScale = Vector3.one * newScale;
            yield return null;
        }
        callBack();
    }

    IEnumerator MovingDown()
    {
        while (myRect.localPosition.y > target)
        {
            Vector3 newPosition = myRect.localPosition;
            newPosition.y -= dropSpeed * Time.deltaTime;
            myRect.localPosition = newPosition;
            yield return null;
        }
        myRect.localPosition = new Vector3(0f, target, 0f);
    }
}
