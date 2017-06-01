using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.UI;

public class MainGameController : MonoBehaviour {
	GameFramework gameFramework;
	GameObject frog;
	GameObject coldFog;
	LevelHandler levelHandler;
	public LevelData levelData;
	public DifficultyManager difficultyManager;
	public ArenaBuilder arenaBuilder;
    public ScoreHandler scoreHandler;
    public UIHandler uiHandler;
    public PowerupHandler powerupHandler;
    public SoundController soundController;
    public string seed;
    public Transform poolParent;
    public TextAsset nouns;
    public TextAsset adjectives;
    public Text LevelNameLabel;
    private IGameProgressReporting newRowScore;

    void Awake()
    {
        Application.targetFrameRate = 45;
        Screen.fullScreen = false;
        SetupGameFramework();
    }

	void Start ()
    {
        levelHandler = gameFramework.BuildGameFramework();
        StartNewGame();
		BuildInitialLevel();
    }

    private void SetupGameFramework()
    {
        gameFramework = new GameFramework();
        gameFramework.arenaBuilder = arenaBuilder;
        gameFramework.difficultyManager = difficultyManager;
        gameFramework.levelData = levelData;
        gameFramework.scoreHandler = scoreHandler;
        gameFramework.poolParent = poolParent;
        gameFramework.powerupHandler = powerupHandler;
        gameFramework.soundController = soundController;
        gameFramework.UI = uiHandler;
    }
		
	private void StartNewGame()
	{
        SessionStatistics.consecutiveRuns++;
        newRowScore = ServiceLocator.getService<IGameProgressReporting>();
        SeedRandomLogger();
		PlaceFrog();
		if(difficultyManager.fogEnabled)
		{
			PlaceColdFogWall();
		}
	}

    private void SeedRandomLogger()
    {
        seed = PlayerPrefs.GetString("Seed");
        if (seed == "")
        {
            seed = GetRandomWord(adjectives.text, 929) + " " + GetRandomWord(nouns.text, 5449);
        }
        RandomLogger.SeedRNG(seed);
        LevelNameLabel.text = seed;
    }

    private string GetRandomWord(String words, int numberOfLines)
    {
        StringReader reader = new StringReader(words);
        int currentLine = 0;
        int targetLine = UnityEngine.Random.Range(1, numberOfLines);
        string word;
        do
        {
            currentLine += 1;
            word = reader.ReadLine();
        }
        while (word != null && currentLine < targetLine);
        return char.ToUpper(word[0]) + word.Substring(1); ;
    }
	
	private void BuildInitialLevel() 
	{
		for (int i = 0; i < levelData.levelLength; ++i)
		{
			levelHandler.buildNewRow();
		}
	}
	
	private void PlaceFrog()
	{
        UnityEngine.Object frogAsset = Resources.Load("FrogCharacter");
		frog = Instantiate(frogAsset, new Vector3 (gameFramework.levelData.levelWidth * 0.5f, -1f, 0f), Quaternion.identity) as GameObject;
		FrogMovementPhysics frogMovementScript = frog.GetComponent<FrogMovementPhysics>();
		frogMovementScript.NewHighestRowReached += NewRowReached;
        FrogController controller = frog.GetComponent<FrogController>();
        controller.OnFrogDeath += HandleFrogDeath;
        controller.OnFoodPickup += uiHandler.UpdateHearts;
        powerupHandler.costumeSwitcher = frog.GetComponent<CostumeSwitcher>();
        powerupHandler.frogController = controller;
        uiHandler.OnSwipeDirectionChange = frog.GetComponent<FrogInputHandler>().SwipeDirectionChange;
        Camera.main.GetComponent<CameraVerticalFollow>().frog = frog;
	}

    void HandleFrogDeath(string causeOfDeath)
    {
        ReportRunStatistics(causeOfDeath);
    }

    private void ReportRunStatistics(string causeOfDeath)
    {
        SessionStatistics.totalTimePlayed += Time.timeSinceLevelLoad;
        Dictionary<string, object> runSummary = new Dictionary<string, object>();
        runSummary.Add("CauseOfDeath", causeOfDeath);
        runSummary.Add("Points", scoreHandler.score);
        runSummary.Add("DificultyLevel", difficultyManager.difficultyLevel);
        runSummary.Add("UsedAdBonus", powerupHandler.usedBonusIncentive);
        runSummary.Add("RunDuration", Time.timeSinceLevelLoad);
        runSummary.Add("PowerupStarts", powerupHandler.totalNumberOfPowerups);
        runSummary.Add("PowerupTime", powerupHandler.totalTimeOnPowerup);
        runSummary.Add("MaxRunAmmo", powerupHandler.maxAmmoThisRun);
        UnityEngine.Analytics.Analytics.CustomEvent("RunSummary", runSummary);
    }

    private void PlaceColdFogWall()
	{
		UnityEngine.Object coldFogAsset = Resources.Load("ColdFog");
		coldFog = Instantiate(coldFogAsset, new Vector3(gameFramework.levelData.levelWidth * 0.5f, levelData.coldFogStartRow, 0f), Quaternion.identity) as GameObject;
        coldFog.name = "ColdFog";
        ColdFogController controller = coldFog.GetComponent<ColdFogController>() as ColdFogController;
        controller.frog = frog;
        controller.levelData = levelData;
    }
	
	IEnumerator restartLevelAterSeconds(float seconds) 
	{
		yield return new WaitForSeconds(seconds);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void RestartGameAfterSeconds(float seconds)
    {
        StartCoroutine(restartLevelAterSeconds(seconds));
    }

    public void RestartGameImmediate()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void SetRandomGameMode()
    {
        PlayerPrefs.SetString("GameMode", "Radom");
        PlayerPrefs.SetString("Seed", "");
        RestartGameImmediate();
    }

    private void NewRowReached(object sender, NewRowReached newRowReachedEventArgs)
	{
		int rowsToBuild = newRowReachedEventArgs.newRowReached - difficultyManager.HighestRowReached;
		for(int i = 0; i < rowsToBuild; ++i)
		{
			levelHandler.buildNewRow();
            StartCoroutine(WaitForNextUpdate());

        }
		difficultyManager.HighestRowReached = newRowReachedEventArgs.newRowReached;
        newRowScore.NewRowsReached(rowsToBuild);
	}
    private IEnumerator WaitForNextUpdate()
    {
        yield return null;
    }

}

