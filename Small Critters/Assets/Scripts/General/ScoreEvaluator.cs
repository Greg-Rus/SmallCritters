using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScoreEvaluator: MonoBehaviour {

    public ScoreEvent[] scoreEvents;
    private Dictionary<string, ScoreEvent> eventMap;

    void Start()
    {
        eventMap = new Dictionary<string, ScoreEvent>();
        for (int i = 0; i < scoreEvents.Length; ++i)
        {
            eventMap.Add(scoreEvents[i].name, scoreEvents[i]);
        }
    }

    public int GetScoreForEvent(string type, int count = 1)
    {
        ScoreEvent scoreEvent = eventMap[type];
        scoreEvent.count += count;
        return scoreEvent.value * count; ;
    }

    public string GetNotificationForEvent(string type)
    {
        ScoreEvent scoreEvent = eventMap[type];
        //++scoreEvent.count;
        return scoreEvent.text;
    }

    public void AddCount(string type, int count)
    {
        ScoreEvent scoreEvent = eventMap[type];
        scoreEvent.count += count;
    }
}
[Serializable]
public class ScoreEvent
{
    public string name = "";
    public string text = "";
    public int value = 0;
    public int count = 0;
}
