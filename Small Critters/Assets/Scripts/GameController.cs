using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public int arenaHeight;
	public int arenaWidth;
	//public FloorPlanLayer floorLayer;
	public GameObject frog;
	public FrogController frogController;
	public Camera mainCamera;
	public Imovement movement;
	public ObstacleSetter obstacleSetter;
	public int maxRowReached;
	public Text score;
	public int newLevelBufferRowCount;
	private int newLevelThreshold;

	// Use this for initialization
	void Start () {
		startGame();
	}
	
	private void startGame()
	{
		arenaHeight = 1;
		placeFrog();
		configureMovementScript();
		configureObstacleSetter();
		configureFrogController();
		buildNextLevel();
	}
	
	public void updateMaxRowReached (int newMaxRowReached)
	{
		if(newMaxRowReached >= newLevelThreshold)
		{
			buildNextLevel();
		}
		maxRowReached = newMaxRowReached;
		//obstacleSetter.dismantleLevelSectionsBelowRowReached(maxRowReached);
		score.text = maxRowReached.ToString();
	}
	private void configureFrogController()
	{
		frogController = frog.GetComponent<FrogController>();
		frogController.myGameController = this;
	}
	
	private void configureMovementScript()
	{
		movement = frog.GetComponent<Imovement>();
		movement.configure(this);
	}
	
	public void configureObstacleSetter()
	{
		obstacleSetter.configure(arenaWidth, arenaHeight);//, floorLayer);
	}
	
	private void placeFrog()
	{
		Vector3 frogStartLocation = new Vector3((arenaWidth * 0.5f)+ 0.5f//floorLayer.basicFloorTile.transform.localScale.x * 0.5f
		                                        ,0f
		                                        ,0f);
		                                        
		frog.transform.position = frogStartLocation;
		frogStartLocation.z = -10f;
		mainCamera.transform.position = frogStartLocation;
		mainCamera.orthographicSize = (arenaWidth + 1) *0.89f;
	}
	
	public void onFrogDeath()
	{
		StartCoroutine(restartLevelAterSeconds(1));
	}
	
	IEnumerator restartLevelAterSeconds(float seconds) 
	{
		yield return new WaitForSeconds(seconds);
		Application.LoadLevel(0);
	}
	public void buildNextLevel()
	{
		arenaHeight = obstacleSetter.buildNextLevel();
		newLevelThreshold = arenaHeight - newLevelBufferRowCount;
		//floorLayer.layLevelFloorAndWalls(arenaHeight);
	}
}
