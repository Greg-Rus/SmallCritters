using UnityEngine;
using System.Collections;

public class ObstacleSetter : MonoBehaviour {
	public int arenaHeight;
	public int arenaWidth;
	public GameObject lineBlades;
	private GameObject nextLineBladesObstacle;
	private Vector3 obstacleSpawnPosition = Vector3.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void configure(int width, int height)
	{
		arenaHeight = height;
		arenaWidth = width+1;
	}
	
	public void initialObstacleDeplayment()
	{
		for (int i = 1 ; i<= arenaHeight; i++)
		{
			layNextObstacle(i);
		}
	}
	
	public void layNextObstacle(int row)
	{
		obstacleSpawnPosition.x = arenaWidth * 0.5f;
		obstacleSpawnPosition.y = row;
		nextLineBladesObstacle = Instantiate ( lineBlades, obstacleSpawnPosition, Quaternion.identity) as GameObject;
		nextLineBladesObstacle.transform.parent = this.gameObject.transform;
		configureLineBlades(nextLineBladesObstacle);
	}
	public void configureLineBlades(GameObject lineBladesObeject)
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
