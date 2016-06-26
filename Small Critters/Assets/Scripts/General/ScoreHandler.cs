using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml.Serialization;

public class ScoreHandler : MonoBehaviour, IDeathReporting, IGameProgressReporting, IScoreForUI
{
    public MainGameController gameController;
    public UIHandler uiHandler;
    public PowerupHandler powerupHandler;
    public int score = 0;
    public int rowMultiplier = 1;
    public int scoringDistance = 3;
    public int beeScoreStarCount = 3;
    public int flyScoreStarCount = 1;
    public int fireBeetleScoreStarCount = 2;
    public int deathViaBlade = 2;
    public int deatchViaBee = 1;
    public int deathViaProcessor = 3;
    public int deathViaVent = 2;
    public int deathViaOther = 1;
    public int starValue = 2;
    public ScoreData scoreData;
    public GameObject star;

    public ScoreEvaluator beeScoreEvaluator;
    public ScoreEvaluator fireBeetleScoreEvaluator;

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

    public void EnemyDead(GameObject enemy, string causeOfDeath)
    {
        int starCount = 0;

        switch (enemy.name)
        {
            case "Bee":           starCount = beeScoreEvaluator.EvaluateKill(causeOfDeath); break;
            case "Fly":           starCount = 1; break;
            case "FireBeetle":    starCount = fireBeetleScoreEvaluator.EvaluateKill(causeOfDeath); break;
        }

        if(starCount > 0) SpawnStars(starCount, enemy.transform.position, 1);
        
    }

    private void SpawnStars(int count, Vector3 position, int points)
    {
        Vector3 offset = Vector3.zero;
        for (int i = 0; i < count; ++i)
        {
            offset.x = UnityEngine.Random.Range(0f, 1f);
            offset.y = UnityEngine.Random.Range(0f, 1f);
            GameObject newStar = Instantiate(star, 
                                            position + offset,
                                            Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360))
                                            ) as GameObject;
            StarHandler newStarHandler = newStar.GetComponent<StarHandler>();
            newStarHandler.Configure(points, scoringDistance, StarCollected);
        }
    }

    public void StarCollected(int points)
    {
        score += points;
        UpdateUIScore();
        powerupHandler.UpdatePoints(points);
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
            bool inserted = false;
            for (int i = 0; i < scoreData.scores.Count; ++i)
            {
                if (newScore.score >= scoreData.scores[i].score)
                {
                    scoreData.scores.Insert(i, newScore);
                    inserted = true;
                    break;
                }
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
    private void UpdateUIPowerupStatus(int points)
    {
        uiHandler.UpdatePowerup(points);
    }

    public void RestartRun(int button)
    {
        PlayerPrefs.SetString("GameMode", "Seeded");
        if (button == -1)
        {
            PlayerPrefs.SetString("Seed", scoreData.lastRun.hash);
            gameController.RestartGame();
        }
        else
        {
            PlayerPrefs.SetString("Seed", scoreData.scores[button].hash);
            gameController.RestartGame();
        }
    }
}
