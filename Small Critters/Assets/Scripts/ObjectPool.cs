using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ObjectPool {
	private GameObject type;
	private int quantity;
	private GameObject[] inactivePool;
	private int nextInactiveObject;
	private int expandAmount;
	private GameObject topInactiveObj;

	public ObjectPool(GameObject objectType, int number, int expand = 50){
		type = objectType;
		quantity = number;
		expandAmount = expand;
		inactivePool = new GameObject[quantity];

		for (int i = 0; i < quantity; i++) {		
			GameObject newObject = GameObject.Instantiate(type, new Vector3(0,0,0), Quaternion.identity) as GameObject;
			newObject.SetActive(false);
			inactivePool[i] = newObject;
		}
		nextInactiveObject = 0;
	}
	
	public GameObject retrieveObject()
	{
		inactivePool [nextInactiveObject].SetActive(true);
		topInactiveObj = inactivePool [nextInactiveObject];
		if (nextInactiveObject == quantity - 1) {
			expandPool();
			nextInactiveObject++;
		}
		else nextInactiveObject++;

		return topInactiveObj;

	}

	public void storeObject(GameObject removedObject)
	{
		nextInactiveObject--;
		removedObject.SetActive(false);
		inactivePool [nextInactiveObject] = removedObject;
	}

	public void expandPool(){
		System.Array.Resize<GameObject> (ref inactivePool, quantity + expandAmount);
		for (int i =quantity; i< quantity+expandAmount; i++) {
			GameObject newObject = GameObject.Instantiate(type, new Vector3(0,0,0), Quaternion.identity) as GameObject;
			newObject.SetActive(false);
			inactivePool[i] = newObject;
		}
		quantity = quantity + expandAmount;
	}
}
