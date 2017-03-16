using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ScoreEvaluator {

    public int deathViaBlade;
    public string NoteTextBlade;
    public int deathViaVent;
    public string NoteTextVent;
    public int deatchViaBee;
    public string NoteTextBee;
    public int deathViaProcessor;
    public string NoteTextProcessor;
    public int deathViaFireBeetle;
    public string NoteTextBeetle;
    public int deathViaOther;
    public string NoteTextOther;
    public int dathViaPellet;

    public int EvaluateKill(string causeOfDeath)
    {
        int starCount = 0;
        switch (causeOfDeath)
        {
            case "Blade":       starCount = deathViaBlade; break;
            case "Flame":       starCount = deathViaVent; break;
            case "Sting":       starCount = deatchViaBee; break;
            case "Processor":   starCount = deathViaProcessor; break;
            case "Pellet":      starCount = dathViaPellet; break;
            case "FlameBall":   starCount = deathViaFireBeetle; break;
            default:            starCount = deathViaOther; break;
        }
        return starCount;
    }

    public string GetNotificationForDeathType(string causeOfDeath)
    {
        switch (causeOfDeath)
        {
            case "Blade": return NoteTextBlade;
            case "Flame": return NoteTextVent;
            case "Sting": return NoteTextBee;
            case "Processor": return NoteTextProcessor;
            case "Pellet": return "shot";
            case "FlameBall": return NoteTextBeetle;
            default: return "";
        }
    }
}
