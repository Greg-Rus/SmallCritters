using UnityEngine;
using System.Collections;

public class FrogInputHandler : MonoBehaviour {
	public bool inputStarted = false;
	public Vector3 startPointerScreenPositoin;
	public Vector3 draggedPointerScreenPosition;
	public Vector3 dragVector;
	private Vector3 pointVector;
    private Vector3 tapVector;
	public float minimalDragDistance;
    private float minimalDragScreenDistance;
	private Imovement frogMovement;
	public JumpLineRenderer jumpLineRenderer;
	public float maxJumpSwipe;
	public float maxJumpLength;
	private float swipeToJumpConversionRatio;
	private Rigidbody2D myRigidbody;
	private Vector3 worldStartPoint;
	private Vector3 worldDraggedPoint;
	private float sqrMouseDistanceFromFrog;
	// Use this for initialization
	void Awake () {
		frogMovement = GetComponent<Imovement>() as Imovement ;
		swipeToJumpConversionRatio = maxJumpLength / maxJumpSwipe;
        //Debug.Log(( Camera.main.ScreenToWorldPoint(new Vector3(10f, 0f, 0f)) - Camera.main.ScreenToWorldPoint(Vector3.zero)).sqrMagnitude);
        minimalDragScreenDistance = (Camera.main.WorldToScreenPoint(new Vector3(minimalDragDistance, 0f, 0f)) - Camera.main.WorldToScreenPoint(Vector3.zero)).sqrMagnitude;
	}
	
	// Update is called once per frame
	void Update()
	{
#if UNITY_STANDALONE
        sqrMouseDistanceFromFrog = (Input.mousePosition - Camera.main.WorldToScreenPoint(this.transform.position)).sqrMagnitude;
        if ((sqrMouseDistanceFromFrog >= minimalDragScreenDistance) && (!frogMovement.midJump))
        {
            if (!jumpLineRenderer.isStarted)
            {
                jumpLineRenderer.setupJumpLine(this.transform.position);
            }
            pointVector = calcualteDragVector(Camera.main.WorldToScreenPoint(this.transform.position), Input.mousePosition);
            frogMovement.rotateToDirection(pointVector);
            jumpLineRenderer.updateJumpLine(pointVector);
        }
        else
        {
            if (jumpLineRenderer.isStarted)
            {
                jumpLineRenderer.stopDrawingJumpLine();
            }
        }
#endif
        if (!frogMovement.midJump)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPointerScreenPositoin = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                draggedPointerScreenPosition = Input.mousePosition;
                dragVector = CalcualteDragVector(startPointerScreenPositoin, draggedPointerScreenPosition);
                if ((draggedPointerScreenPosition - startPointerScreenPositoin).sqrMagnitude > minimalDragScreenDistance)
                {
                    if (!jumpLineRenderer.isStarted)
                    {
                        jumpLineRenderer.setupJumpLine(this.transform.position);
                    }
                    frogMovement.rotateToDirection(dragVector);
                    jumpLineRenderer.updateJumpLine(dragVector);
                }
                else
                {
                    if (jumpLineRenderer.isStarted)
                    {
                        jumpLineRenderer.stopDrawingJumpLine();
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (dragVector.sqrMagnitude > Mathf.Pow(minimalDragDistance, 2f))
                {
                    frogMovement.rotateToDirection(dragVector);
                    frogMovement.makeMove(dragVector);
                }
                jumpLineRenderer.stopDrawingJumpLine();
            }

        }
//Old
        //if (!frogMovement.midJump)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        startPointerScreenPositoin = Input.mousePosition;
        //    }

        //    if (Input.GetMouseButton(0))
        //    {
        //        draggedPointerScreenPosition = Input.mousePosition;
        //        if ((draggedPointerScreenPosition - startPointerScreenPositoin).sqrMagnitude > minimalDragScreenDistance)
        //        {
        //            if (!jumpLineRenderer.isStarted)
        //            {
        //                jumpLineRenderer.setupJumpLine(this.transform.position);
        //            }
        //            dragVector = CalcualteDragVector(startPointerScreenPositoin, draggedPointerScreenPosition);
        //            frogMovement.rotateToDirection(dragVector);
        //            jumpLineRenderer.updateJumpLine(dragVector);
        //        }
        //        else
        //        {
        //            if (jumpLineRenderer.isStarted)
        //            {
        //                jumpLineRenderer.stopDrawingJumpLine();
        //            }
        //        }
        //    }
        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        dragVector = CalcualteDragVector(startPointerScreenPositoin, draggedPointerScreenPosition);
        //        if (dragVector.sqrMagnitude > Mathf.Pow(minimalDragDistance, 2f))
        //        {
        //            frogMovement.rotateToDirection(dragVector);
        //            frogMovement.makeMove(dragVector);
        //        }
        //        else if (pointVector.sqrMagnitude > Mathf.Pow(minimalDragDistance, 2f))
        //        {
        //            frogMovement.rotateToDirection(pointVector);
        //            frogMovement.makeMove(pointVector);
        //        }
        //        else
        //        {
        //            tapVector = CalcualteDragVector(Camera.main.WorldToScreenPoint(this.transform.position), startPointerScreenPositoin);
        //            frogMovement.rotateToDirection(tapVector);
        //            frogMovement.makeMove(tapVector);
        //        }
        //        jumpLineRenderer.stopDrawingJumpLine();
        //    }
        //}
        
	}

	Vector3 CalcualteDragVector(Vector3 start, Vector3 end)
	{
		CalculateSwipesWorldCoordinates(start, end);
		
		dragVector = (worldDraggedPoint - worldStartPoint) * -1f;
		if(dragVector.magnitude <= maxJumpSwipe)
		{
			return dragVector * swipeToJumpConversionRatio;
		}
		else
		{
			return dragVector.normalized * maxJumpSwipe * swipeToJumpConversionRatio;
		}
	}
	void CalculateSwipesWorldCoordinates(Vector3 start, Vector3 end)
	{
		worldStartPoint = Camera.main.ScreenToWorldPoint(start);
		worldStartPoint.z = 0f;
		worldDraggedPoint = Camera.main.ScreenToWorldPoint(end);
		worldDraggedPoint.z = 0f;
	}
}
