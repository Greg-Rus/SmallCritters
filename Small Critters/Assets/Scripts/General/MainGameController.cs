using UnityEngine;
using System.Collections;

public class MainGameController : MonoBehaviour {
	GameFramework gameFramework;
	GameObject frog;
	// Use this for initialization
	void Start () {
		gameFramework.BuildGameFramework();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void StartNewGame()
	{
		ResetGame();
		PlaceFrog();
	}
	
	private void ResetGame()
	{
		
	}
	
	private void PlaceFrog()
	{
		frog = Resources.Load("Frog") as GameObject;
		FrogMovementPhysics frogMovementScript = GetComponent<FrogMovementPhysics>();
		//frogMovementScript.NewHighestRowReached += OnNewHighestRowReached;
	}
	
	private void OnNewHighestRowReached()
	{
	
	}
}
