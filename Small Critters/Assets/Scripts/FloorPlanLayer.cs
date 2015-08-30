﻿using UnityEngine;
using System.Collections;

public class FloorPlanLayer : MonoBehaviour {

	public GameObject basicFloorTile;
	public GameObject wallTile;
	public int arenaHeight;
	public int arenaWidth;
	GameObject newFloorTile;
	Vector3 tilePosition = Vector3.zero;
	
	// Use this for initialization
	void Start () {
		//layFloorPlan();
	}
	
	// Update is called once per frame
	public void layFloorPlan(int width, int height)
	{
		arenaHeight = height+1;
		arenaWidth = width+1;
		
		tilePosition = Vector3.zero;
		for (int i = 0 ; i<= arenaWidth; i++)
		{
			for (int j =0 ; j <= arenaHeight ; j++)
			{
				tilePosition.x = i;
				tilePosition.y = j;
				tilePosition.z = this.transform.position.z;
				if (i == 0 || i == arenaWidth)
				{
					newFloorTile = Instantiate(wallTile, tilePosition, Quaternion.identity) as GameObject;
				}
				else
				{
					newFloorTile = Instantiate(basicFloorTile, tilePosition, Quaternion.identity) as GameObject;
				}
				newFloorTile.transform.parent = this.transform;
			}
		}
		//Vector3 recenteredFloorPosition = new Vector3((arenaWidth * -0.5f) + basicFloorTile.transform.localScale.x * 0.5f, 
		//                                              (arenaHeight * -0.5f) + basicFloorTile.transform.localScale.y * 0.5f,
		//                                              this.transform.position.z);
		//this.transform.position = recenteredFloorPosition;
	}
	public void layNextArenaRow(int rowPosition)
	{

		for (int i = 0 ; i<= arenaWidth; i++)
		{
			tilePosition.x = i;
			tilePosition.y = rowPosition;
			tilePosition.z = this.transform.position.z;
			if (i == 0 || i == arenaWidth)
			{
				newFloorTile = Instantiate(wallTile, tilePosition, Quaternion.identity) as GameObject;
			}
			else
			{
				newFloorTile = Instantiate(basicFloorTile, tilePosition, Quaternion.identity) as GameObject;
			}
			newFloorTile.transform.parent = this.transform;
		}
	}
}
