﻿using UnityEngine;
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
    public NotificationHandler myNotifications;
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
    public float summarySpawnSpeed;
    public ScoreEvaluator beeScoreEvaluator;
    public ScoreEvaluator fireBeetleScoreEvaluator;
    public ScoreEvaluatorShots shotsScoreEvaluator;
    public ScoreEvaluator generalScoreEvaluator;

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

    public void NewRowsReached(int rowsTraversed)
    {
        score += generalScoreEvaluator.GetScoreForEvent("Distance", rowsTraversed);
        UpdateUIScore();
    }

    public void EnemyDead(GameObject enemy, string causeOfDeath)
    {
        int starCount = 0;
        if (causeOfDeath == "Pellet")
        {
            starCount = shotsScoreEvaluator.GetScoreForEvent(causeOfDeath);
            ProcessNotification(shotsScoreEvaluator.GetNotificationForEvent(causeOfDeath));
        }
        else
        {
            switch (enemy.name)
            {
                case "Bee":
                    starCount = beeScoreEvaluator.GetScoreForEvent(causeOfDeath);
                    ProcessNotification(beeScoreEvaluator.GetNotificationForEvent(causeOfDeath)); break;
                case "Fly": starCount = generalScoreEvaluator.GetScoreForEvent("Fly"); break;
                case "FireBeetle":
                    starCount = fireBeetleScoreEvaluator.GetScoreForEvent(causeOfDeath);
                    ProcessNotification(fireBeetleScoreEvaluator.GetNotificationForEvent(causeOfDeath)); break;
            }
        }

        if (starCount > 0)
        {
            score += starCount;
            UpdateUIScore();
            SpawnStars(starCount, enemy.transform.position);
        } 
    }

    //private NotificationType CauseOfDeathToNotificationType(string causeOfDeath)
    //{
    //    switch (causeOfDeath)
    //    {
    //        case "Blade": return NotificationType.BladeKill;
    //        case "Flame": return NotificationType.VentKill;
    //        case "Sting": return NotificationType.OtherBee;
    //        case "Processor": return NotificationType.ProcessorKill;
    //        case "Pellet": return NotificationType.SingleKill;
    //        case "FlameBall": return NotificationType.OtherBeetle;
    //        case "ColdFog": return NotificationType.ColdKill;
    //        default: Debug.LogError("Unsupported cause of death: " + causeOfDeath); return NotificationType.Undefined;
    //    }
    //}

    private void ProcessNotification(string notification)
    {
        myNotifications.ShowNotification(notification);
    }

    private void SpawnStars(int count, Vector3 position)
    {
        Vector3 offset = Vector3.zero;
        for (int i = 0; i < count; ++i)
        {
            offset.x = UnityEngine.Random.Range(-0.5f, 0.5f);
            offset.y = UnityEngine.Random.Range(-0.5f, 0.5f);
            GameObject newStar = Instantiate(star, 
                                            position + offset,
                                            Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360))
                                            ) as GameObject;
            StarHandler newStarHandler = newStar.GetComponent<StarHandler>();
            newStarHandler.Configure(scoringDistance, StarCollected);
        }
    }

    public void StarCollected()
    {
        int starValue = generalScoreEvaluator.GetScoreForEvent("Star");
        score += starValue;
        UpdateUIScore();
        powerupHandler.UpdatePoints(starValue);
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
        StartCoroutine(PresentRunSummary());
    }

    IEnumerator PresentRunSummary()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(1f);
        bool runHadEvents = false;
        foreach (ScoreEvent se in generalScoreEvaluator.scoreEvents)
        {
            if (se.count > 0)
            {
                uiHandler.AddSummaryItem(se.text, se.count, se.value, true);
                runHadEvents = true;
                yield return new WaitForSecondsRealtime(summarySpawnSpeed);
            }
        }
        foreach (ScoreEvent se in shotsScoreEvaluator.scoreEvents)
        {
            if (se.count > 0)
            {
                uiHandler.AddSummaryItem(se.text, se.count, se.value, true);
                runHadEvents = true;
                yield return new WaitForSecondsRealtime(summarySpawnSpeed);
            }
        }
        foreach (ScoreEvent se in fireBeetleScoreEvaluator.scoreEvents)
        {
            if (se.count > 0)
            {
                uiHandler.AddSummaryItem(se.text, se.count, se.value, true);
                runHadEvents = true;
                yield return new WaitForSecondsRealtime(summarySpawnSpeed);
            }
        }
        foreach (ScoreEvent se in beeScoreEvaluator.scoreEvents)
        {
            if (se.count > 0)
            {
                uiHandler.AddSummaryItem(se.text, se.count, se.value, true);
                runHadEvents = true;
                yield return new WaitForSecondsRealtime(summarySpawnSpeed);
            }
        }
        if (runHadEvents)
        {
            uiHandler.OnSummaryCompositionFinished();
        }
        else
        {
            gameController.RestartGameAfterSeconds(0.5f);
        }
        
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
            gameController.RestartGameImmediate();
        }
        else
        {
            PlayerPrefs.SetString("Seed", scoreData.scores[button].hash);
            gameController.RestartGameImmediate();
        }
    }
    public void RestartGame()
    {
        gameController.RestartGameImmediate();
    }

    public void SpeedUpSummary()
    {
        summarySpawnSpeed *= 0.2f;
    }


}
