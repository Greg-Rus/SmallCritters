using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameObjectPoolManager{
	private Dictionary<string,ObjectPool> pools;
	private Transform parentForObjects;
	
	public GameObjectPoolManager(Transform parentForObjects = null)
	{
		pools = new Dictionary<string, ObjectPool>();
		this.parentForObjects = parentForObjects;
	}
	
	public ObjectPool addPool(GameObject objectType, int number, int expand = 50)
	{
		
		if(!pools.ContainsKey(objectType.name))
		{
			ObjectPool newPool = new ObjectPool(objectType, number, expand, parentForObjects);
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
