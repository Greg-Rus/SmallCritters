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
	private int lastEmptyRow = -1;
	private int consecutiveEmptyRows = 0;
	private List<List<GameObject>> bufferedRows;
	public WallBuilder wallSectionBuilder;
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
		if(row.Count == 0)
		{
			if(lastEmptyRow == (levelData.levelTop - 1))
			{
				++consecutiveEmptyRows;
				if(consecutiveEmptyRows == 4)
				{
					wallSectionBuilder.BuildWallSegament(row, new Vector2(1.5f, levelData.levelTop-3), 3, 4, 5, true);
					lastEmptyRow = -1; 
					consecutiveEmptyRows = 0;
				}
			}
			lastEmptyRow = levelData.levelTop;
		}
		else
		{
			lastEmptyRow = -1; 
			consecutiveEmptyRows = 0;
		}
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
		wallSectionBuilder.poolManager = poolManager;
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
		GameObject newTile = poolManager.retrieveObject(prefab.name);
		newTile.transform.position = tilePosition;
        SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>();
        renderer.sprite = tileSprite;
       // renderer.color = new Color32(255,255,200,255);
        currentRow.Add(newTile);
	}
}
