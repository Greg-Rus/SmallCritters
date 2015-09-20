using UnityEngine;
using System.Collections;

public class ObstacleSetter : MonoBehaviour {
	public int arenaHeight;
	public int arenaWidth;
	public GameObject lineBlades;
	public GameObject processor;
	private GameObject nextLineBladesObstacle;
	private Vector3 obstacleSpawnPosition = Vector3.zero;
	public float processorCycleOffset = 0f;
	private float processorCycleStage = 0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	
	public void configure(int width, int height)
	{
		arenaHeight = height;
		arenaWidth = width+1;
	}
	
	public void initialObstacleDeplayment()
	{
		for (int i = 1 ; i<= arenaHeight; i++)
		{
# if (UNITY_EDITOR)
			layTestObstacle(i);
# else		
			layNextObstacle(i);
# endif
		}
	}
	
	public void layNextObstacle(int row)
	{
		if(Random.Range(1,4) >1)
		{
			obstacleSpawnPosition.x = arenaWidth * 0.5f;
			obstacleSpawnPosition.y = row;
			nextLineBladesObstacle = Instantiate ( lineBlades, obstacleSpawnPosition, Quaternion.identity) as GameObject;
			nextLineBladesObstacle.transform.parent = this.gameObject.transform;
			configureLineBlades(nextLineBladesObstacle);
		}

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
	public void layTestObstacle(int row)
	{
		layProcessorRow(row);
	}
	private void layProcessorRow(int row)
	{
		for (int i = 1; i < arenaWidth ; i++)
		{
			Vector3 processorPosition;
			processorPosition.x = i;
			processorPosition.y = row;
			processorPosition.z = this.transform.position.z;
			GameObject newProcessor = Instantiate(processor, processorPosition, Quaternion.identity) as GameObject;
			newProcessor.transform.parent = this.transform;
			
			processorCycleStage = ((processorCycleStage + processorCycleOffset > 1f) ? 0f : processorCycleStage + processorCycleOffset);
			Debug.Log (processorCycleStage);
			newProcessor.GetComponent<ProcessorHeater>().setProcessorState(processorCycleStage);
		}
	}
	
}
