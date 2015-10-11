using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameObjectPoolManager{
	private Dictionary<string,ObjectPool> pools;
	
	public GameObjectPoolManager()
	{
		pools = new Dictionary<string, ObjectPool>();
	}
	
	public ObjectPool addPool(GameObject objectType, int number, int expand = 50)
	{
		
		if(!pools.ContainsKey(objectType.name))
		{
			ObjectPool newPool = new ObjectPool(objectType, number, expand);
			pools.Add(objectType.name,newPool);
			return newPool;
		}
		else
		{
			Debug.LogError("Pool with for objects named >> " + objectType.name + " << already exists");
			return pools[objectType.name];
		}
		

	}
	public GameObject retrieveObject(string name)
	{
		return pools[name].retrieveObject();
	}
	
	public void storeObject(GameObject objectToStore)
	{
		if(objectToStore == null)
		{
			Debug.Log("null object passed!");
		}
		if(pools[objectToStore.name] == null)
		{
			Debug.Log("No pool for name: " + objectToStore.name);
		}
		pools[objectToStore.name].storeObject(objectToStore);
	}
}
