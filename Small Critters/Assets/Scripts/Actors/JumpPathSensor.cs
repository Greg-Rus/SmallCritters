using UnityEngine;
using System.Collections;

public class JumpPathSensor : MonoBehaviour {
	private int hazardInTrigger = 0;
	private BoxCollider2D jumpPathCollider;
	
	void Awake()
	{
		jumpPathCollider = GetComponent<BoxCollider2D>();
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
			++hazardInTrigger;
	}
	void OnTriggerExit2D(Collider2D other)
	{
			--hazardInTrigger;
	}
	public bool isTouchingHazard()
	{
		return (hazardInTrigger > 0) ? true : false;
	}
	public void reset()
	{
		hazardInTrigger = 0;
	}
	public bool checkForHazardsInJumpPath(Vector3 direction)
	{
		resizeJumpPathCollider(direction);
		return isTouchingHazard();
	}
	
	private void resizeJumpPathCollider(Vector3 direction)
	{
		Vector2 newJumpPathColliderOffset = jumpPathCollider.offset;
		newJumpPathColliderOffset.y = -1 * direction.magnitude *0.5f;
		jumpPathCollider.offset = newJumpPathColliderOffset;
		
		Vector2 newJumpPathColliderSize = jumpPathCollider.size;
		newJumpPathColliderSize.y = direction.magnitude;
		jumpPathCollider.size = newJumpPathColliderSize;
	}
	
}
