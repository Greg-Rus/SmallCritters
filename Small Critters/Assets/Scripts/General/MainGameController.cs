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
    public UIHandler uiHandler;
    public PowerupHandler powerupHandler;
    public string seed = "42";
    public Transform poolParent;
    public TextAsset nouns;
    public TextAsset adjectives;
    public Text LevelNameLabel;
    public int daysToRemindMovementTutorial;
   // public static MainGameController instance;
    // Use this for initialization
    void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.fullScreen = false;
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
        //PlayerPrefs.SetInt("LastGameDay", System.DateTime.Today.DayOfYear);
        //PlayerPrefs.DeleteKey("LastGameDay");
        //PlayerPrefs.SetInt("LastGameYear", System.DateTime.Today.Year);
        //Debug.Log(Application.persistentDataPath);
        //PlayerPrefs.DeleteAll();
        //levelData = new LevelData();
        //difficultyManager = GetComponentInChildren<DifficultyManager>();
        difficultyManager.levelData = levelData;
        //gameFramework = new GameFramework(levelData, difficultyManager, arenaBuilder);

        SetupGameFramework();
        levelHandler = gameFramework.BuildGameFramework();
        AddMonoBehaviourServices();
        StartNewGame();
		BuildInitialLevel();
        DisplayTutorial();
        
    }

    private void AddMonoBehaviourServices()
    {
        ServiceLocator.addService<PowerupHandler>(powerupHandler);
    }

    private void DisplayTutorial()
    {
        int lastGameDay = PlayerPrefs.GetInt("LastGameDay");
        if (lastGameDay == 0)
        {
            PlayerPrefs.SetInt("LastGameDay", System.DateTime.Today.DayOfYear);
            uiHandler.ShowTutorial();
        }
        else if (PlayerPrefs.GetInt("LastGameDay") <= System.DateTime.Today.AddDays(-daysToRemindMovementTutorial).DayOfYear)
        {
            uiHandler.ShowTutorial();
        }
    }

    //void Update()
    //{
    //    Time.timeScale = timescale;
    //}
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
        //SeedRNG();
        SeedRandomLogger();
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
            //seed = RandomLogger.GetRandomRange(this,0, 9999999).ToString();
            seed = GetRandomWord(adjectives.text, 929) + " " + GetRandomWord(nouns.text, 5449);
           //Debug.Log("New random seed: " + seed);
        }
        
        UnityEngine.Random.seed = seed.GetHashCode();
        LevelNameLabel.text = seed;
        //Debug.Log("New random seed: " + seed);
    }
    private void SeedRandomLogger()
    {
        seed = PlayerPrefs.GetString("Seed");
        if (seed == "")
        {
            seed = GetRandomWord(adjectives.text, 929) + " " + GetRandomWord(nouns.text, 5449);
        }
        RandomLogger.SeedRNG(seed);
        //UnityEngine.Random.seed = seed.GetHashCode();
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
        powerupHandler.costumeSwitcher = frog.GetComponent<CostumeSwitcher>();
        //frog.GetComponentInChildren<ShotgunController>().powerupHandler = powerupHandler;
        FrogController controller = frog.GetComponent<FrogController>();
        //controller.FrogDeath += HandleFrogDeath;
        //controller.FrogDeath += scoreHandler.RunEnd;
        controller.OnFrogDeath += HandleFrogDeath;
        controller.OnFrogDeath += scoreHandler.RunEnd;
        controller.OnFoodPickup += uiHandler.UpdateHearts;
        uiHandler.OnSwipeDirectionChange = frog.GetComponent<FrogInputHandler>().SwipeDirectionChange;
        Camera.main.GetComponent<CameraVerticalFollow>().frog = frog;
	}

    //void HandleFrogDeath (object sender, EventArgs e)
    //{
    //	StartCoroutine(restartLevelAterSeconds(1));
    //}

    void HandleFrogDeath(string causeOfDeath)
    {
        Time.timeScale = 0.5f;
        StartCoroutine(restartLevelAterSeconds(1f));
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
        //Application.LoadLevel(0);
        //RandomLogger.SaveAndClose();
        RestartGame();

    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void NewRowReached(object sender, NewRowReached newRowReachedEventArgs)
	{
		int rowsToBuild = newRowReachedEventArgs.newRowReached - difficultyManager.HighestRowReached;
		for(int i = 0; i < rowsToBuild; ++i)
		{
			levelHandler.buildNewRow();
            StartCoroutine("WaitForNextUpdate");

        }
		difficultyManager.HighestRowReached = newRowReachedEventArgs.newRowReached;
        scoreHandler.NewRowsReached(rowsToBuild);
	}
    private IEnumerator WaitForNextUpdate()
    {
        yield return null;
    }

}

