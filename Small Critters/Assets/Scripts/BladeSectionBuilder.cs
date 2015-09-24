using UnityEngine;
using System.Collections;

public class BladeSectionBuilder : MonoBehaviour {
	private int currentArenaHeight;
	private int newArenaHeight;
	private Vector3 obstacleSpawnPosition = Vector3.zero;
	private GameObject nextLineBladesObstacle;
	public int arenaWidth;
	public GameObject lineBlades;
	
	// Use this for initialization
	void Start () {
	
	}
	
	public void buildBladeSection(int fromRow, int toRow)
	{
		Debug.Log ("From: " + fromRow + "  " + "To: " + toRow);
		currentArenaHeight = fromRow;
		newArenaHeight = toRow;
		
		for (int i = currentArenaHeight ; i <= newArenaHeight ; i++)
		{
			layNextBladeRow(i);
		}
	}
	
	public void layNextBladeRow(int row)
	{
		if(Random.Range(1,4) >1)
		{
			obstacleSpawnPosition.x = arenaWidth * 0.5f;
			obstacleSpawnPosition.y = row;
			nextLineBladesObstacle = Instantiate ( lineBlades, obstacleSpawnPosition, Quaternion.identity) as GameObject;
			nextLineBladesObstacle.transform.parent = this.gameObject.transform;
			configureBlades(nextLineBladesObstacle);
		}
		
	}
	
	public void configureBlades(GameObject lineBladesObeject)
	{
		LineBladesMovement lineBladesScript = lineBladesObeject.GetComponent<LineBladesMovement>();
		if (Random.Range(0,2) >0)
		{
			lineBladesObeject.transform.Rotate(new Vector3(0f,0f,180f));
		}
		lineBladesScript.bladeSpeed = Random.Range(1,3);
		lineBladesScript.gap = Random.Range(3,6);
		lineBladesScript.setupBlades();
		lineBladesScript.preWarmFan(Random.Range(1,100));
		
	}
	

}
