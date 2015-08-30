using UnityEngine;
using System.Collections;

public class FrogInputHandler : MonoBehaviour {
	public bool inputStarted = false;
	public Vector3 startPointerPositoin;
	public Vector3 draggedPointerPosition;
	public Vector3 dragVector;
	public float minimalDragDistance;
	public Imovement frogMovement;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			inputStarted= true;
			startPointerPositoin = Input.mousePosition;
		}
		
		if (Input.GetMouseButton(0))
		{
			draggedPointerPosition = Input.mousePosition;
			calcualteDragVector();
		}
		
		if (Input.GetMouseButtonUp(0))
		{
			inputStarted= false;
			dragVector = calcualteDragVector();
			if(dragVector.magnitude > minimalDragDistance)
			{
				frogMovement.makeMove(calcualteDragVector());
			}
			else
			{
				//TODO frogMovement.tap(draggedPointerPosition);
			}
			
		}
	}
	Vector3 calcualteDragVector()
	{
		return draggedPointerPosition - startPointerPositoin;
	}
}
