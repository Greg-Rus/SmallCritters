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

    public int GetScoreForKill(string type)
    {
        ScoreEvent scoreEvent = eventMap[type];
        return scoreEvent.value;
    }

    public string GetNotificationForKill(string type)
    {
        ScoreEvent scoreEvent = eventMap[type];
        ++scoreEvent.count;
        return scoreEvent.text;
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
