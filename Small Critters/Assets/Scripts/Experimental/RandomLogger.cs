using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class RandomLogger : MonoBehaviour {
	public static int seed;
	private static string values;
	// Use this for initializations

	
	// Update is called once per frame
	public static void SeedRNG(string seedString)
	{
		
		seed = seedString.GetHashCode ();
		Random.seed = seed;
	}

	public static int GetRandomRange( int min, int max)
	{
		int result = Random.Range (min, max);
		values += result + ",";
		return result;
	}
	public static void SaveAndClose()
	{
		System.IO.File.AppendAllText (@"c:\RNGLog.txt", values);
	}
}
