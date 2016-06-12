using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallBuilder : MonoBehaviour {
	private List<Vector2> wallMap;
	public int wallMapX = 3;
	public int wallMapY = 4;
	public int wallElements = 5;
	private Vector2 currentTile;
	private Vector2[] offsets = {new Vector2(0,1), new Vector2(0,-1), new Vector2(1,0), new Vector2(-1,0)};
	private Vector2[] edges = {new Vector2(0,1), new Vector2(1,1), new Vector2(1,0), new Vector2(1,-1), new Vector2(0,-1), new Vector2(-1,-1), new Vector2(-1,0), new Vector2(-1,1)};
	private List<Vector2> legalOffsets;
	private Vector2 offset;
	public WallTile[] wallTiles;
	public GameObject wallPrefab;
	private Dictionary<int, Sprite> wallTilesDict;
	private Vector2 wallSegmentOrigin;
	public GameObjectPoolManager poolManager;
	private List<GameObject> currentRow;

	void Awake () {
		wallMap = new List<Vector2>();
		legalOffsets = new List<Vector2>();
		wallTilesDict = new Dictionary<int, Sprite>();
		foreach(WallTile tile in wallTiles)
		{
			wallTilesDict.Add (tile.hash, tile.sprite);
		}	
	}
	
	public void BuildWallSegament(List<GameObject> row, Vector2 position, int width, int height, int numberOfBlocks, bool isMirrored = false)
	{
		currentRow = row;
		wallSegmentOrigin = position;
		wallMapX = width;
		wallMapY = height;
		wallElements = numberOfBlocks;
		MapWalls();
		SetUpWalls();
		if(isMirrored)
		{
			MirrorTileMap();
		}
	}
	
	private void MirrorTileMap()
	{
		float tableMid = wallMapX * 0.5f;
		int distanceToMid = Mathf.FloorToInt(tableMid);
		int pivot = Mathf.CeilToInt(tableMid);
		for(int i = 0; i < wallMap.Count; ++i)
		{
			Vector2 tilePosition = wallMap[i];
			float newX = (distanceToMid - tilePosition.x) + pivot;
			tilePosition.x = newX + 3; //TODO If levelData is available make this Mathf.Ceil(navigableArenaWidth*0.5)
			wallMap[i] = tilePosition;
		}
		SetUpWalls();
	}
	
	private void SetUpWalls()
	{
	
		foreach(Vector2 v in wallMap)
		{
			GameObject newTile = poolManager.retrieveObject(wallPrefab.name);
			newTile.transform.position = v + wallSegmentOrigin;
			newTile.GetComponent<SpriteRenderer>().sprite = wallTilesDict[GetTileHash(v)];
			currentRow.Add (newTile);
		}
	}
	
	private void MapWalls()
	{
		wallMap.Clear();
		currentTile = new Vector2(Random.Range(0,wallMapX), Random.Range(0,wallMapY));
		wallMap.Add(currentTile);
		for(int i = 0; i< wallElements; ++i)
		{
			currentTile = PickWallLocation(currentTile);
			if(!wallMap.Contains(currentTile))
			{
				wallMap.Add(currentTile);
			}
		}
	}
	private Vector2 PickWallLocation(Vector2 currentTile)
	{
		return currentTile + GetTileOffset();
	}
	
	private Vector2 GetTileOffset()
	{
		legalOffsets.Clear();
		legalOffsets.AddRange(offsets);
		if(currentTile.x == 0)
		{
			legalOffsets.RemoveAt(3);
		}
		else if(currentTile.x == wallMapX-1)
		{
			legalOffsets.RemoveAt(2);
		}
		if(currentTile.y == 0)
		{
			legalOffsets.RemoveAt(1);
		}
		else if(currentTile.y == wallMapY-1)
		{
			legalOffsets.RemoveAt(0);
		}

		if(legalOffsets.Count == 0)
		{
			currentTile = wallMap[Random.Range(0,wallMap.Count)];
			GetTileOffset();
		}
		offset = legalOffsets[Random.Range(0,legalOffsets.Count)];
		return offset;
	}
	private int GetTileHash(Vector2 location)
	{
		int hash = 0;
		int bitPosition = 0;
		foreach(Vector2 edge in edges)
		{
			Vector2 neighbour = location + edge;
			if (wallMap.Contains(neighbour))
			{
				hash = hash | (int)(Mathf.Pow(2, bitPosition));
			}
			++bitPosition;
		}
		if(((hash & 5) != 5) && ((hash & 2) == 2)) hash = hash ^ 2;
		if(((hash & 20) != 20) && ((hash & 8) == 8)) hash = hash ^ 8;
		if(((hash & 80) != 80) && ((hash & 32) == 32)) hash = hash ^ 32;
		if(((hash & 65) != 65) && ((hash & 128) == 128)) hash = hash ^ 128;

		return hash;
	}
}
[System.Serializable]
public class WallTile
{
	public int hash;
	public Sprite sprite;
}
