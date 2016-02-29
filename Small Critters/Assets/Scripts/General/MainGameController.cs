using UnityEngine;
using System.Collections;
using System;

public class MainGameController : MonoBehaviour {
	GameFramework gameFramework;
	GameObject frog;
	GameObject coldFog;
	LevelHandler levelHandler;
	public LevelData levelData;
	public DifficultyManager difficultyManager;
	// Use this for initialization
	void Start () {
		//levelData = new LevelData();
		difficultyManager = GetComponent<DifficultyManager>();
		difficultyManager.levelData = levelData;
		gameFramework = new GameFramework(levelData, difficultyManager);
		levelHandler = gameFramework.BuildGameFramework();
		StartNewGame();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void StartNewGame()
	{
		ResetGame();
		BuildInitialLevel();
		PlaceFrog();
		if(difficultyManager.fogEnabled)
		{
			PlaceColdFogWall();
		}
		
	}
	
	private void BuildInitialLevel() 
	{
		
		for (int i = 0; i < levelData.levelLength; ++i)
		{
			levelHandler.buildNewRow();
		}
	}
	
	private void ResetGame()
	{
		
	}
	
	private void PlaceFrog()
	{
		UnityEngine.Object frogAsset = Resources.Load("Frog"); // as GameObject;
		frog = Instantiate(frogAsset, new Vector3 (gameFramework.levelData.levelWidth * 0.5f, -1f, 0f), Quaternion.identity) as GameObject;
		FrogMovementPhysics frogMovementScript = frog.GetComponent<FrogMovementPhysics>();
		frogMovementScript.NewHighestRowReached += NewRowReached;
		frog.GetComponent<FrogController>().FrogDeath += HandleFrogDeath;
		Camera.main.GetComponent<CameraVerticalFollow>().frog = frog;
	}

	void HandleFrogDeath (object sender, EventArgs e)
	{
		StartCoroutine(restartLevelAterSeconds(1));
	}
	
	private void PlaceColdFogWall()
	{
		UnityEngine.Object coldFogAsset = Resources.Load("ColdFog");
		coldFog = Instantiate(coldFogAsset, new Vector3(gameFramework.levelData.levelWidth * 0.5f, -10f, 0f), Quaternion.identity) as GameObject;
		coldFog.GetComponent<ColdFogController>().frog = frog; 
	}
	
	IEnumerator restartLevelAterSeconds(float seconds) 
	{
		yield return new WaitForSeconds(seconds);
		Application.LoadLevel(0);
	}
	
	private void NewRowReached(object sender, NewRowReached newRowReachedEventArgs)
	{
		//TODO hook this up to UI score
		int rowsToBuild = newRowReachedEventArgs.newRowReached - difficultyManager.HighestRowReached;
		for(int i = 0; i < rowsToBuild; ++i)
		{
			levelHandler.buildNewRow();
		}
		difficultyManager.HighestRowReached = newRowReachedEventArgs.newRowReached;
		 //TODO can't call this once per event as the player could have juped several rows!! Calculate the number of calls
		//TODO The frog should be placed in the middle of the level. Wait untill row 25 is reached befor calling for new row or make first 25 rows empty and place the frog at 24.
	}

}

