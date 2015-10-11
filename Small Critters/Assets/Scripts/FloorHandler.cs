﻿using UnityEngine;
using System.Collections;

public class FloorHandler : MonoBehaviour {
	public float floorShiftOffset;
	public MeshRenderer leftWallMeshRenderer;
	public MeshRenderer rightWallMeshRenderer;
	public int wallOrderInLayer;
	
	void Start()
	{
		leftWallMeshRenderer.sortingOrder = wallOrderInLayer;
		rightWallMeshRenderer.sortingOrder = wallOrderInLayer;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		Vector3 newPosition = this.transform.position;
		newPosition.y = newPosition.y + floorShiftOffset;
		this.transform.position = newPosition;
	}
}
