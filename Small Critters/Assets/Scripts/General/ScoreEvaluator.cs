using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ScoreEvaluator {

    public int deathViaBlade;
    public int deathViaVent;
    public int deatchViaBee;
    public int deathViaProcessor;
    public int deathViaFireBeetle;
    public int deathViaOther;
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
}
