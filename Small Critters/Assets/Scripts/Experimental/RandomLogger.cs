using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using MersenneTwister;

public class RandomLogger : MonoBehaviour {
	public static int seed;
    private static RandomMT RNG;

	public static void SeedRNG(string seedString)
	{
		seed = seedString.GetHashCode ();
        RNG = new RandomMT((ulong)seed);
	}

	public static int GetRandomRange(MonoBehaviour script, int min, int max) //TODO Phase the uses of script loggin out
	{
        int result = RNG.RandomRange(min, max);
		return result;
	}

    public static int GetRandomRange(int min, int max)
    {
        int result = RNG.RandomRange(min, max);
        return result;
    }


    public static float GetRandomRange(MonoBehaviour script, float min, float max)
    {
        float result = RNG.RandomRange(min, max);
        return result;
    }
    public static float GetRandomRange(float min, float max)
    {
        float result = RNG.RandomRange(min, max);
        return result;
    }

    public static bool RollBelowPercent(MonoBehaviour script, float pecent)
    {
        float result = RNG.RandomRange(0, 100);
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
