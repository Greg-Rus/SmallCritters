using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaBuilder : MonoBehaviour, IArenaBuilding {
	public GameObject floorTilePrefab;
	public GameObject sideWallTilePrefab;
	public Sprite[] floorSprites;
	public Sprite[] wallSprites;
    public GameObject bladeRowWallLeft;
    public GameObject bladeRowWallRight;
	private Vector3 tilePosition;
	private List<GameObject> currentRow;
	private LevelData levelData;
	private GameObjectPoolManager poolManager;
	public WallBuilder wallSectionBuilder;
    public Color oddTileColor;
    public Color processorSectionColor;
    public Color bladeSectionColor;
    public Color beeSectionColor;
    public Color ventSectionColor;

	public void SetUpArenaRow(List<GameObject> row)
	{
		currentRow = row;
        SetupFloor();
        SetupSideWalls();
        
	}
	
	public void Setup(LevelData levelData, GameObjectPoolManager poolManager)
	{
		tilePosition = Vector3.zero;
		this.levelData = levelData;
		this.poolManager = poolManager;
		poolManager.addPool(sideWallTilePrefab, 100);
		poolManager.addPool(floorTilePrefab, 300);
        poolManager.addPool(bladeRowWallLeft, 20);
        poolManager.addPool(bladeRowWallRight, 20);
        wallSectionBuilder.poolManager = poolManager;
	}

    private void SetupFloor()
    {
        for (float i = levelData.leftWallX + 1f; i < levelData.navigableAreaWidth + 1f; ++i)
        {
            Sprite newFloorSprite = floorSprites[Random.Range(0, floorSprites.Length)];
            SetNewTilePosition(i, levelData.levelTop);
            if (((i - 0.5f) % 2) == 0)
            {
                SpawnTile(floorTilePrefab, newFloorSprite, Color.white);
            }
            else
            {
                SpawnTile(floorTilePrefab, newFloorSprite, oddTileColor);
            }
        }
    }

	private void SetNewTilePosition(float x, float y)
	{
		tilePosition.x = x;
		tilePosition.y = y;
	}
	private void SetupSideWalls()
	{
        if (levelData.activeSectionBuilder.type == SectionBuilderType.blade)
        {
            if (levelData.emptyRow)
            {
                BuildStandardSideWalls();
            }
            else
            {
                BuildBladeRowWalls();
            }
            levelData.emptyRow = false;
        }
        else
        {
            BuildStandardSideWalls();
        }
	}

    private void BuildStandardSideWalls()
    {
        SetNewTilePosition(levelData.leftWallX, levelData.levelTop+1);
        SpawnTile(sideWallTilePrefab, wallSprites[0]);

        SetNewTilePosition(levelData.rightWallX, levelData.levelTop+1);
        SpawnTile(sideWallTilePrefab, wallSprites[1]);
    }

    private void BuildBladeRowWalls()
    {
        SetNewTilePosition(levelData.leftWallX, levelData.levelTop + 1);
        SpawnTile(bladeRowWallLeft);

        SetNewTilePosition(levelData.rightWallX, levelData.levelTop + 1);
        SpawnTile(bladeRowWallRight);
    }
	
	private void SpawnTile(GameObject prefab, Sprite tileSprite)
	{
		GameObject newTile = poolManager.retrieveObject(prefab.name);
		newTile.transform.position = tilePosition;
        SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>();
        renderer.sprite = tileSprite;
        currentRow.Add(newTile);
	}
    private void SpawnTile(GameObject prefab, Sprite tileSprite, Color color)
    {
        GameObject newTile = poolManager.retrieveObject(prefab.name);
        newTile.transform.position = tilePosition;
        SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>();
        renderer.sprite = tileSprite;
        renderer.color = color;
        currentRow.Add(newTile);
    }

    private void SpawnTile(GameObject prefab)
    {
        GameObject newTile = poolManager.retrieveObject(prefab.name);
        newTile.transform.position = tilePosition;
        currentRow.Add(newTile);
    }

}
