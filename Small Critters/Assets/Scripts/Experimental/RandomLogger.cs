using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using MersenneTwister;

public class RandomLogger : MonoBehaviour {
	public static int seed;
	//private static string values;
    private static RandomMT RNG;
	// Use this for initializations
    

	
	// Update is called once per frame
	public static void SeedRNG(string seedString)
	{
		seed = seedString.GetHashCode ();
        //values += seedString + ",";
        //values += seed + ",";
        RNG = new RandomMT((ulong)seed);
        //Random.seed = seed;
	}

	public static int GetRandomRange(MonoBehaviour script, int min, int max)
	{
        Debug.Log(RNG);
        //int result = Random.Range (min, max);
        Debug.Log(script.name);
        int result = RNG.RandomRange(min, max);
		//values += script.name+"-"+result+ ",";
		return result;
	}
    public static float GetRandomRange(MonoBehaviour script, float min, float max)
    {
        float result = RNG.RandomRange(min, max);
        //values += script.name + "-" + result + ",";
        return result;
    }
    public static void SaveAndClose()
	{
        //values += "\n";

        //System.IO.File.AppendAllText (@"K:\RNGLog.csv", values);
	}

    public static bool RollBelowPercent(MonoBehaviour script, float pecent)
    {
        float result = RNG.RandomRange(0, 100);
        //values += script.name + "-" + result + ",";
        if (result <= pecent * 100f)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
