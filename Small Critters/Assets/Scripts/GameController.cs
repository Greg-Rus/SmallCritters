using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public int arenaHeight;
	public int arenaWidth;
	public FloorPlanLayer floorLayer;
	public GameObject frog;
	public Camera mainCamera;
	// Use this for initialization
	void Start () {
		floorLayer.layFloorPlan(arenaWidth, arenaHeight);
		placeFrog();
	}
	
	private void placeFrog()
	{
		Vector3 frogStartLocation = new Vector3((arenaWidth * 0.5f)+floorLayer.basicFloorTile.transform.localScale.x * 0.5f
		                                        ,0f
		                                        ,0f);
		                                        
		frog.transform.position = frogStartLocation;
		frogStartLocation.z = -10f;
		mainCamera.transform.position = frogStartLocation;
	}
	public void onMoveUp()
	{
		//floorLayer.
	}

}
