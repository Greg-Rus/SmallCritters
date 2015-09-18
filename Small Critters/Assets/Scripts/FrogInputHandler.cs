using UnityEngine;
using System.Collections;

public class FrogInputHandler : MonoBehaviour {
	public bool inputStarted = false;
	public Vector3 startPointerPositoin;
	public Vector3 draggedPointerPosition;
	public Vector3 dragVector;
	public float minimalDragDistance;
	public Imovement frogMovement;
	public JumpLineRenderer jumpLineRenderer;
	public float maxJumpSwipe; //TODO Need to indicate to the player the max jump distance based on swipe using jumpLineRenderer
	public float maxJumpLength;
	private float swipeToJumpConversionRatio;
	private Rigidbody2D myRigidbody;
	private Vector3 worldStartPoint;
	private Vector3 worldDraggedPoint;
	// Use this for initialization
	void Awake () {
		//frogMovement = GetComponent<Imovement>() as Imovement ;
		swipeToJumpConversionRatio = maxJumpLength / maxJumpSwipe;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			inputStarted= true;
			startPointerPositoin = Input.mousePosition;
			jumpLineRenderer.setupJumpLine(worldStartPoint);
		}
		
		if (Input.GetMouseButton(0))
		{
			draggedPointerPosition = Input.mousePosition;
			frogMovement.rotateToDirection(calcualteDragVector()); //Frog faces the drag direction during dragging
			jumpLineRenderer.updateJumpLine(calcualteDragVector());
		}
		
		if (Input.GetMouseButtonUp(0))
		{
			inputStarted= false;
			dragVector = calcualteDragVector();
			if(dragVector.magnitude > minimalDragDistance)
			{
				frogMovement.makeMove(calcualteDragVector());
				jumpLineRenderer.stopDrawingJumpLine();
			}
			else
			{
				jumpLineRenderer.stopDrawingJumpLine();
				//TODO frogMovement.tap(draggedPointerPosition);
			}
			
		}
	}
	Vector3 calcualteDragVector()
	{
		calculateSwipesWorldCoordinates();
		
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
	void calculateSwipesWorldCoordinates()
	{
		worldStartPoint = Camera.main.ScreenToWorldPoint(startPointerPositoin);
		worldStartPoint.z = 0f;
		worldDraggedPoint = Camera.main.ScreenToWorldPoint(draggedPointerPosition);
		worldDraggedPoint.z = 0f;
	}
}
