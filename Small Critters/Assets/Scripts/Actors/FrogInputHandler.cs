using UnityEngine;
using System.Collections;

public class FrogInputHandler : MonoBehaviour {
	public bool inputStarted = false;
	public Vector3 startPointerPositoin;
	public Vector3 draggedPointerPosition;
	public Vector3 dragVector;
	private Vector3 pointVector;
	public float minimalDragDistance;
	public Imovement frogMovement;
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
		//frogMovement = GetComponent<Imovement>() as Imovement ;
		swipeToJumpConversionRatio = maxJumpLength / maxJumpSwipe;
	}
	
	// Update is called once per frame
	void Update()
	{
		sqrMouseDistanceFromFrog = (Input.mousePosition - Camera.main.WorldToScreenPoint (this.transform.position)).sqrMagnitude;
		if ((sqrMouseDistanceFromFrog >= 0.25f) && (!frogMovement.midJump)) {
			if (!jumpLineRenderer.isStarted) 
			{
				jumpLineRenderer.setupJumpLine (this.transform.position);
			}
			pointVector = calcualteDragVector (Camera.main.WorldToScreenPoint (this.transform.position), Input.mousePosition);
			frogMovement.rotateToDirection (pointVector);
			jumpLineRenderer.updateJumpLine (pointVector);
		} else {
			if (jumpLineRenderer.isStarted) {
				jumpLineRenderer.stopDrawingJumpLine ();
			}
		}

		if (Input.GetMouseButtonDown (0) && !frogMovement.midJump) {
			startPointerPositoin = Input.mousePosition;
		}
		if (Input.GetMouseButton (0) && !frogMovement.midJump) {
			draggedPointerPosition = Input.mousePosition;
			if ((draggedPointerPosition - startPointerPositoin).sqrMagnitude > 0.25f) 
			{
				if (!jumpLineRenderer.isStarted) 
				{
					jumpLineRenderer.setupJumpLine (this.transform.position);
				}
				dragVector = calcualteDragVector (startPointerPositoin, draggedPointerPosition);
				frogMovement.rotateToDirection (dragVector);
				jumpLineRenderer.updateJumpLine (dragVector);
			}
			else 
			{
				if (jumpLineRenderer.isStarted) 
				{
					jumpLineRenderer.stopDrawingJumpLine ();
				}
			}
		}
		if (Input.GetMouseButtonUp (0) && !frogMovement.midJump) 
		{
			dragVector = calcualteDragVector (startPointerPositoin, draggedPointerPosition);
			if (dragVector.sqrMagnitude > 0) 
			{
				frogMovement.rotateToDirection (dragVector);
				frogMovement.makeMove (dragVector);
			} 
			else if (pointVector.sqrMagnitude > 0) 
			{
				frogMovement.rotateToDirection (pointVector);
				frogMovement.makeMove (pointVector);
			}
			jumpLineRenderer.stopDrawingJumpLine();
		}
	}

	Vector3 calcualteDragVector(Vector3 start, Vector3 end)
	{
		calculateSwipesWorldCoordinates(start, end);
		
		dragVector = worldDraggedPoint - worldStartPoint;
		if(dragVector.magnitude <= maxJumpSwipe)
		{
			return dragVector * swipeToJumpConversionRatio;
		}
		else
		{
			return dragVector.normalized * maxJumpSwipe * swipeToJumpConversionRatio;
		}
	}
	void calculateSwipesWorldCoordinates(Vector3 start, Vector3 end)
	{
		worldStartPoint = Camera.main.ScreenToWorldPoint(start);
		worldStartPoint.z = 0f;
		worldDraggedPoint = Camera.main.ScreenToWorldPoint(end);
		worldDraggedPoint.z = 0f;
	}
}
