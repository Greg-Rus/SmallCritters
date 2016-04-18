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
	public float maxJumpSwipe;
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
		if (Input.GetMouseButtonDown(0) && !frogMovement.midJump)
		{
			//inputStarted= true;
			startPointerPositoin = Input.mousePosition;
            frogMovement.rotateToDirection(calcualteDragVector(Camera.main.WorldToScreenPoint(this.transform.position), startPointerPositoin));
            jumpLineRenderer.setupJumpLine(worldStartPoint);
		}
		
		if (Input.GetMouseButton(0) && !frogMovement.midJump)
        {
            if (startPointerPositoin == Vector3.zero)
            {
                startPointerPositoin = Input.mousePosition;
                jumpLineRenderer.setupJumpLine(worldStartPoint);
            }
            draggedPointerPosition = Input.mousePosition;
            if ((draggedPointerPosition - startPointerPositoin).sqrMagnitude > 0)
            {
                frogMovement.rotateToDirection(calcualteDragVector(startPointerPositoin, draggedPointerPosition)); //Frog faces the drag direction during dragging
               
            }
            jumpLineRenderer.updateJumpLine(calcualteDragVector(startPointerPositoin, draggedPointerPosition));

        }
		
		if (Input.GetMouseButtonUp(0) && !frogMovement.midJump)
		{
			//inputStarted= false;
			dragVector = calcualteDragVector(startPointerPositoin, draggedPointerPosition);

            if (dragVector.magnitude > minimalDragDistance)
            {
                frogMovement.makeMove(dragVector);
                jumpLineRenderer.stopDrawingJumpLine();
            }
            else
            {
                startPointerPositoin = Camera.main.WorldToScreenPoint( this.transform.position);
                draggedPointerPosition = Input.mousePosition;
                dragVector = calcualteDragVector(startPointerPositoin, draggedPointerPosition);
                frogMovement.rotateToDirection(dragVector);
                frogMovement.makeMove(dragVector);
                jumpLineRenderer.stopDrawingJumpLine();
                //TODO frogMovement.tap(draggedPointerPosition);
            }
            startPointerPositoin = Vector3.zero;
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
