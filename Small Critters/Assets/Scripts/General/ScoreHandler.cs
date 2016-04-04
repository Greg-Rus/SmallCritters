using UnityEngine;
using System.Collections;
using System;
using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Binary;

public class ScoreHandler : MonoBehaviour {
    public MainGameController gameController;
    public UIHandler uiHandler;
    public int score = 0;
    public int rowMultiplier = 1;
    public int scoringDistance = 7;
    public int beeScore = 5;

    public int deathViaBlade = 2;
    public int deatchViaBee = 1;
    public int deathViaProcessor = 3;
    public int deathViaVent = 2;

    public ScoreData scoreData;

    void Awake()
    {
        //Debug.Log("Last Run: " + scoreData.lastRun.hash + " score: " + scoreData.lastRun.score);
        //if (scoreData == null)
        //{
        //    Debug.Log("Making new scoreData");
        //    scoreData = new ScoreData();
        //}

        LoadScores();
    }

    private void SaveScores()
    {
        BinaryFormatter foromatter = new BinaryFormatter();

        FileStream scoresFile = File.Create(Application.persistentDataPath + "/scores.dat");
        foromatter.Serialize(scoresFile, scoreData);
        scoresFile.Close();
    }

    private void LoadScores()
    {
        //Debug.Log(Application.persistentDataPath);
        if (File.Exists(Application.persistentDataPath + "/scores.dat"))
        {
            BinaryFormatter foromatter = new BinaryFormatter();
            FileStream scoresFile = File.Open(Application.persistentDataPath + "/scores.dat", FileMode.Open);
            scoreData = foromatter.Deserialize(scoresFile) as ScoreData;
        }
    }

    public void NewRowsReached(int rows)
    {
        score += rows * rowMultiplier;
        UpdateUIScore();
    }

    public void EnemyDead(string enemy, string causeOfDeath)
    {
        int enemyTypeScore = 0;
        switch (enemy)
        {
            case "Bee": enemyTypeScore = beeScore; break;
        }
        int causeOfDeathMultiplier = 0;
        switch (causeOfDeath)
        {
            case "Blade":       causeOfDeathMultiplier = deathViaBlade; break;
            case "HeatVent":    causeOfDeathMultiplier = deathViaVent; break;
            case "Bee":         causeOfDeathMultiplier = deatchViaBee; break;
            case "Processor":   causeOfDeathMultiplier = deathViaProcessor; break;
        }
        score += enemyTypeScore * causeOfDeathMultiplier;
        UpdateUIScore();
    }

    public void RunEnd(string cuseOfDeath)
    {
        Score newScore = new Score(gameController.seed, score);
        scoreData.lastRun = newScore;

        if (scoreData.scores.Count == 0)
        {
            scoreData.scores.Add(newScore);
        }
        else
        {
            //int scoreListIndex = 0;
            //Debug.Log("scoreData.scores.Count: " + scoreData.scores.Count);
            bool inserted = false;
            for(int i = 0; i < scoreData.scores.Count; ++i)
            //foreach (Score scoreEntry in scoreData.scores)
            {
                if (newScore.score >= scoreData.scores[i].score)
                {
                    scoreData.scores.Insert(i, newScore);
                   // Debug.Log("Inserting at: " + i);
                    inserted = true;
                    break;
                }
               // ++scoreListIndex;
            }
            if (!inserted)
            {
                scoreData.scores.Add(newScore);
            }
           
            if (scoreData.scores.Count > 10)
            {
                scoreData.scores.RemoveAt(10);
            }
        }
        SaveScores();
    }

    public ScoreData GetScoreData()
    {
        return scoreData;
    }



    private void UpdateUIScore()
    {
        uiHandler.UpdateUIScore(score);
    }
}
