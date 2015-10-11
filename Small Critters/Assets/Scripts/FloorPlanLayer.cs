using UnityEngine;
using System.Collections;

public class FloorPlanLayer : MonoBehaviour {

	public GameObject basicFloorTile;
	public GameObject wallTile;
	public int arenaWidth;
	public int currentArenaHeight;
	GameObject newFloorTile;
	Vector3 tilePosition = Vector3.zero;
	GameObject[] sceneryElements;
	GameObjectPoolManager pools;
	
	// Use this for initialization
	void Start () {
		//layFloorPlan();
	}
	public void configure(int width, GameObjectPoolManager pools)
	{
		arenaWidth = width;
		currentArenaHeight = -1;
		this.pools = pools;
		pools.addPool(wallTile,120 *2);
		pools.addPool(basicFloorTile, width * 60 *2);
		
	}
	
	// Update is called once per frame
/*	public void layLevelFloorAndWalls(int targetArenaHeight)
	{
		tilePosition = Vector3.zero;
		for (int i = currentArenaHeight+1 ; i<= targetArenaHeight; i++)
		{
			layNextArenaRow(i);
		}
	}
*/
	public GameObject[] layNextArenaRow(int rowPosition)
	{
		sceneryElements = new GameObject[arenaWidth+1];
		for (int i = 0 ; i<= arenaWidth; i++)
		{
			tilePosition.x = i;
			tilePosition.y = rowPosition;
			tilePosition.z = this.transform.position.z;
			if (i == 0 || i == arenaWidth)
			{
				newFloorTile = pools.retrieveObject("Wall");
				//newFloorTile.transform.position = tilePosition;
				//newFloorTile = Instantiate(wallTile, tilePosition, Quaternion.identity) as GameObject;
			}
			else
			{
				newFloorTile = pools.retrieveObject("BasicFloor");
				//newFloorTile.transform.position = tilePosition;
				//newFloorTile = Instantiate(basicFloorTile, tilePosition, Quaternion.identity) as GameObject;
			}
			newFloorTile.transform.position = tilePosition;
			newFloorTile.transform.parent = this.transform;
			sceneryElements[i] = newFloorTile;
		}
		return sceneryElements;
	}
}
