using UnityEngine;
using System.Collections;

public class FloorPlanLayer : MonoBehaviour {

	public GameObject basicFloorTile;
	public GameObject wallTile;
	public int arenaWidth;
	public int currentArenaHeight;
	GameObject newFloorTile;
	Vector3 tilePosition = Vector3.zero;
	
	// Use this for initialization
	void Start () {
		//layFloorPlan();
	}
	public void configure(int width)
	{
		arenaWidth = width+1;
		currentArenaHeight = -1;
	}
	
	// Update is called once per frame
	public void layLevelFloorAndWalls(int targetArenaHeight)
	{
		tilePosition = Vector3.zero;
		for (int i = currentArenaHeight+1 ; i<= targetArenaHeight; i++)
		{
			layNextArenaRow(i);
		}
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
