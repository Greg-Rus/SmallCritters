using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaBuilder : MonoBehaviour, IArenaBuilding {
	public GameObject floorTilePrefab;
	public GameObject sideWallTilePrefab;
//	private string floorPrefabName;
//	private string wallPrefabName;
	public Sprite[] floorSprites;
	public Sprite[] wallSprites;
	private Vector3 tilePosition;
	private List<GameObject> currentRow;
	private LevelData levelData;
	private GameObjectPoolManager poolManager;
	// Use this for initialization
	void Start () {
//		tilePosition = new Vector2();
//		for(int i = 0; i<10;++i)
//		{
//			SetUpArenaRow(i);
//		}
	}


	public void SetUpArenaRow(List<GameObject> row)
	{
		currentRow = row;
		SetupSideWalls();
		SetupFloor();
	}
	
	public void Setup(LevelData levelData, GameObjectPoolManager poolManager)
	{
		tilePosition = Vector3.zero;
		this.levelData = levelData;
		this.poolManager = poolManager;
		poolManager.addPool(sideWallTilePrefab, 100);
		poolManager.addPool(floorTilePrefab, 300);
	}
	
	private void SetupFloor()
	{
		for(float i = 1.5f; i < levelData.navigableAreaWidth+1 ; ++i)
		{
			Sprite newFloorSprite = floorSprites[Random.Range(0,floorSprites.Length)];
			SetNewTilePosition(i,levelData.levelTop);
			SpawnTile(floorTilePrefab, newFloorSprite);
		}
	}
	private void SetNewTilePosition(float x, float y)
	{
		tilePosition.x = x;
		tilePosition.y = y;
	}
	private void SetupSideWalls()
	{
		SetNewTilePosition(0.5f, levelData.levelTop);
		SpawnTile(sideWallTilePrefab, wallSprites[0]);
		
		SetNewTilePosition(8.5f, levelData.levelTop);
		SpawnTile(sideWallTilePrefab, wallSprites[1]);
	}
	
	private void SpawnTile(GameObject prefab, Sprite tileSprite)
	{
		//GameObject newTile = Instantiate(prefab, tilePosition, Quaternion.identity) as GameObject; 
		GameObject newTile = poolManager.retrieveObject(prefab.name);
		newTile.transform.position = tilePosition;
		newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
		currentRow.Add(newTile);
	}
}
