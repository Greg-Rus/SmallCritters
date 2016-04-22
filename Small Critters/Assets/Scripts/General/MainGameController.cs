using UnityEngine;
using System.Collections;
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
    public string seed = "42";
    public Transform poolParent;
    public TextAsset nouns;
    public TextAsset adjectives;
    public Text LevelNameLabel;
   // public static MainGameController instance;
    // Use this for initialization
    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else if(instance != this)
        //{
        //    Destroy(gameObject);
        //}
    }

	void Start () {
        //Debug.Log(Application.persistentDataPath);
        //PlayerPrefs.DeleteAll();
		//levelData = new LevelData();
		//difficultyManager = GetComponentInChildren<DifficultyManager>();
		difficultyManager.levelData = levelData;
        //gameFramework = new GameFramework(levelData, difficultyManager, arenaBuilder);

        SetupGameFramework();
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
    }
		
	private void StartNewGame()
	{
		ResetGame();
        SeedRNG();
		//
		PlaceFrog();
		if(difficultyManager.fogEnabled)
		{
			PlaceColdFogWall();
		}
		
	}

    private void SeedRNG()
    {
        seed = PlayerPrefs.GetString("Seed");
        //Debug.Log("On Start PlayerPrefs seed: " + seed);
        if (seed == "")
        {
            //seed = UnityEngine.Random.Range(0, 9999999).ToString();
            seed = GetRandomWord(adjectives.text, 929) + " " + GetRandomWord(nouns.text, 5449);
           //Debug.Log("New random seed: " + seed);
        }
        
        UnityEngine.Random.seed = seed.GetHashCode();
        LevelNameLabel.text = seed;
        //Debug.Log("New random seed: " + seed);
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
	
	private void ResetGame()
	{
		
	}
	
	private void PlaceFrog()
	{
        //UnityEngine.Object frogAsset = Resources.Load("Frog"); // as GameObject;
        UnityEngine.Object frogAsset = Resources.Load("FrogCharacter");
		frog = Instantiate(frogAsset, new Vector3 (gameFramework.levelData.levelWidth * 0.5f, -1f, 0f), Quaternion.identity) as GameObject;
		FrogMovementPhysics frogMovementScript = frog.GetComponent<FrogMovementPhysics>();
		frogMovementScript.NewHighestRowReached += NewRowReached;
        FrogController controller = frog.GetComponent<FrogController>();
        //controller.FrogDeath += HandleFrogDeath;
        //controller.FrogDeath += scoreHandler.RunEnd;
        controller.OnFrogDeath += HandleFrogDeath;
        controller.OnFrogDeath += scoreHandler.RunEnd;
        Camera.main.GetComponent<CameraVerticalFollow>().frog = frog;
	}

	//void HandleFrogDeath (object sender, EventArgs e)
	//{
	//	StartCoroutine(restartLevelAterSeconds(1));
	//}
    void HandleFrogDeath(string causeOfDeath)
    {
        StartCoroutine(restartLevelAterSeconds(1));
    }

    private void PlaceColdFogWall()
	{
		UnityEngine.Object coldFogAsset = Resources.Load("ColdFog");
		coldFog = Instantiate(coldFogAsset, new Vector3(gameFramework.levelData.levelWidth * 0.5f, -25f, 0f), Quaternion.identity) as GameObject;
        coldFog.name = "ColdFog";
        ColdFogController controller = coldFog.GetComponent<ColdFogController>() as ColdFogController;
        controller.frog = frog;
        controller.levelData = levelData;

    }
	
	IEnumerator restartLevelAterSeconds(float seconds) 
	{
		yield return new WaitForSeconds(seconds);
        //Application.LoadLevel(0);
        RestartGame();

    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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
        scoreHandler.NewRowsReached(rowsToBuild);
		 //TODO can't call this once per event as the player could have juped several rows!! Calculate the number of calls
		//TODO The frog should be placed in the middle of the level. Wait untill row 25 is reached befor calling for new row or make first 25 rows empty and place the frog at 24.
	}

}

