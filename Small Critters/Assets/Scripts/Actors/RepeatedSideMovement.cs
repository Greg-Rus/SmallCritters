using UnityEngine;
using System.Collections;

public class RepeatedSideMovement : MonoBehaviour {
    public Vector2 start;
    public Vector2 end;
    public float speed;
    private Vector3 movementOffset;
    private float time = 0;
    public Transform myTransform;
    public Transform parentTransform;
    private Vector3 parentOffset;

    void Start()
    {
        parentOffset = parentTransform.position - myTransform.position;
    }
	
	void Update () {
        SetOffsetByLerping();
        UpdateLerpTime();
        UpdatePosition();
    }
    private void SetOffsetByLerping()
    {
        movementOffset = Vector2.LerpUnclamped(start, end, time);
    }

    private void UpdateLerpTime()
    {
        time += Time.deltaTime * speed;
        if (time >= 1) time = 0;
    }

    private void UpdatePosition()
    {
        Vector3 newPosition = parentTransform.position;
        newPosition = newPosition + movementOffset - parentOffset;
        myTransform.position = newPosition;
    }
}
