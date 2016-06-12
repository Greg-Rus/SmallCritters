//using UnityEngine;
//using System.Collections;

//public class LevelSection{
//	public int sectionStart;
//	public int sectionEnd;
//	public GameObject[,] sectionObjects;
//	public ObjectPool objectPool;
	
//	public LevelSection(int start, int end, GameObject[,] objects, ObjectPool pool)
//	{
//		sectionStart = start;
//		sectionEnd = end;
//		sectionObjects = objects;
//		objectPool = pool;
//	}
	
//	public void dismantleSection()
//	{
//		foreach(GameObject item in sectionObjects)
//		{
//			objectPool.storeObject(item);
//		}
//	}
//}
