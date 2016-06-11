using UnityEngine;
using System.Collections;

public class JumpPathSensor : MonoBehaviour {
	private BoxCollider2D jumpPathCollider;
	public LayerMask hazardousLayer;
	
	void Awake()
	{
		jumpPathCollider = GetComponent<BoxCollider2D>();
	}

    public bool checkForHazardsInJumpPath(Vector3 direction)
	{
		resizeJumpPathCollider(direction);
		return jumpPathCollider.IsTouchingLayers();
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
