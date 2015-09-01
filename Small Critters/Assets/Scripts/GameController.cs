using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public int arenaHeight;
	public int arenaWidth;
	public FloorPlanLayer floorLayer;
	public GameObject frog;
	public Camera mainCamera;
	public Imovement movement;
	public ObstacleSetter obstacleSetter;
	// Use this for initialization
	void Start () {
		floorLayer.configure(arenaWidth, arenaHeight);
		floorLayer.layInitialFloorPlan();
		placeFrog();
		configureMovementScript();
		configureObstacleSetter();
		obstacleSetter.initialObstacleDeplayment();
	}
	
	private void configureMovementScript()
	{
		movement = frog.GetComponent<Imovement>();
		movement.configure(this);
	}
	
	public void configureObstacleSetter()
	{
		obstacleSetter.configure(arenaWidth, arenaHeight);
	}
	
	private void placeFrog()
	{
		Vector3 frogStartLocation = new Vector3((arenaWidth * 0.5f)+floorLayer.basicFloorTile.transform.localScale.x * 0.5f
		                                        ,0f
		                                        ,0f);
		                                        
		frog.transform.position = frogStartLocation;
		frogStartLocation.z = -10f;
		mainCamera.transform.position = frogStartLocation;
		mainCamera.orthographicSize = (arenaWidth + 1) *0.89f;
	}
	public void onMoveUp()
	{
		++arenaHeight;
		floorLayer.layNextArenaRow(arenaHeight);
		obstacleSetter.layNextObstacle(arenaHeight);
	}

}
