using UnityEngine;
using System.Collections;

public class BladeSectionBuilder : MonoBehaviour {
	private int currentArenaHeight;
	private int newArenaHeight;
	private Vector3 obstacleSpawnPosition = Vector3.zero;
	private GameObject nextLineBladesObstacle;
	public int arenaWidth;
	private GameObjectPoolManager pools;
	public GameObject lineBlades;
	public ObstacleSetter myObstacleSetter;
	private LevelRow newLevelRow;
	
	// Use this for initialization
	void Start () {
	
	}
	public void configure(int newArenaWidth, GameObjectPoolManager newPools, ObstacleSetter mySetter)
	{
		arenaWidth = newArenaWidth;
		pools = newPools;
		pools.addPool(lineBlades, 100);
		pools.addPool(lineBlades.GetComponent<LineBladesMovement>().blade,300);
		myObstacleSetter = mySetter;
	}
	
	public void buildBladeSection(int fromRow, int toRow)
	{
		currentArenaHeight = fromRow;
		newArenaHeight = toRow;
		
		for (int i = currentArenaHeight ; i <= newArenaHeight ; i++)
		{
			layNextBladeRow(i);
		}
	}
	
	public void layNextBladeRow(int row)
	{
		newLevelRow = myObstacleSetter.getBaseLevelRow(row);
		if(Random.Range(1,4) >1)
		{
			obstacleSpawnPosition.x = arenaWidth * 0.5f;
			obstacleSpawnPosition.y = row;
			nextLineBladesObstacle = pools.retrieveObject("LineBlades");
			nextLineBladesObstacle.transform.position = obstacleSpawnPosition;
			//nextLineBladesObstacle = Instantiate ( lineBlades, obstacleSpawnPosition, Quaternion.identity) as GameObject;
			//nextLineBladesObstacle.transform.parent = this.gameObject.transform;
			GameObject[] usedBladeObjects = configureBlades(nextLineBladesObstacle);
			GameObject[] allUsedObjects = new GameObject[usedBladeObjects.Length + 1];
			allUsedObjects[0] = nextLineBladesObstacle;
			usedBladeObjects.CopyTo(allUsedObjects,1);
			
			newLevelRow.addHazard(allUsedObjects);
		}
		myObstacleSetter.addNewFinishedLevelRow(newLevelRow);
	}
	
	public GameObject[] configureBlades(GameObject lineBladesObeject)
	{
		LineBladesMovement lineBladesScript = lineBladesObeject.GetComponent<LineBladesMovement>();
		if (Random.Range(0,2) >0)
		{
			lineBladesObeject.transform.Rotate(new Vector3(0f,0f,180f));
		}
		lineBladesScript.bladeSpeed = Random.Range(1,3);
		lineBladesScript.gap = Random.Range(3,6);
		GameObject[] usedObjects = lineBladesScript.setupBlades(pools);
		lineBladesScript.preWarmFan(Random.Range(1,100));
		return usedObjects;
	}
	

}
