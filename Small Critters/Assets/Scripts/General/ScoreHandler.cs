using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Runtime.Serialization.Formatters.Binary;
//using Polenter.Serialization;
using System.Xml.Serialization;

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

    XmlSerializer serializer;

    void Awake()
    {
        serializer = new XmlSerializer(typeof(ScoreData));
        LoadScores();
    }

    private void SaveScores()
    {
        FileStream scoresFile = File.Create(Application.persistentDataPath + "/scores.dat");
        serializer.Serialize(scoresFile, scoreData);
        scoresFile.Close();
    }

    private void LoadScores()
    {
        if (File.Exists(Application.persistentDataPath + "/scores.dat"))
        {
            FileStream scoresFile = File.Open(Application.persistentDataPath + "/scores.dat", FileMode.Open);
            scoreData = serializer.Deserialize(scoresFile) as ScoreData;
            scoresFile.Close();
        }
        else
        {
            scoreData = new ScoreData();
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
        //Debug.Log("RUN END");
        Score newScore = new Score(gameController.seed, score);
        scoreData.lastRun = newScore;

        if (scoreData.scores.Count == 0)
        {
            //scoreData.scores.Add(newScore);
            Debug.Log("Adding: " + newScore.hash + ", " + newScore.score + " at BEGINING");
            scoreData.scores.Add(newScore);
        }
        else
        {
            //int scoreListIndex = 0;
            //Debug.Log("scoreData.scores.Count: " + scoreData.scores.Count);
            bool inserted = false;
            //for (LinkedListNode<Score> node = scoreData.scores.First; node != null; node = node.Next)
            //{
            //    if (newScore.score >= node.Value.score)
            //    {
            //        scoreData.scores.AddBefore(node, newScore);
            //        inserted = true;
            //        break;
            //    }
            //}
            for (int i = 0; i < scoreData.scores.Count; ++i)
            //foreach (Score scoreEntry in scoreData.scores)
            {
                if (newScore.score >= scoreData.scores[i].score)
                {
                    //Debug.Log("Inserting: " + newScore.hash + ", " + newScore.score + " at: " + i);
                    scoreData.scores.Insert(i, newScore);
                    // Debug.Log("Inserting at: " + i);
                    inserted = true;
                    break;
                }
                // ++scoreListIndex;
            }
            if (!inserted)
            {
                //Debug.Log("Appending: " + newScore.hash + ", " + newScore.score + " at END");
                scoreData.scores.Add(newScore);
                //scoreData.scores.AddLast(newScore);
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

    public void RestartRun(int button)
    {
        if (button == -1)
        {
            PlayerPrefs.SetString("Seed", scoreData.lastRun.hash);
            gameController.RestartGame();
        }
        else
        {
            //int buttonNumber = int.Parse(button.name);
            Debug.Log("Button number: " + button);
            Debug.Log("Target hash: " + scoreData.scores[button].hash);
            PlayerPrefs.SetString("Seed", scoreData.scores[button].hash);
            gameController.RestartGame();
        }
    }
}
