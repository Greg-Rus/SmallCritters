//using UnityEngine;
//using System.Collections;

//public class LineBladesMovement : MonoBehaviour {
//	public GameObject blade;
//	public GameObject emitter;
//	public GameObject receiver;
//	public float gap;
//	public float bladeSpeed;
//	public Vector3 normalizedVectorToReceiver;
//	public Vector3 normalizedVectorToEmitter;
//	public Vector3 vectorToReceiver;
//	public float bladeLength;
//	public int numberOfBlades;
//	public float distanceBetweenBladeCenters;
//	public  GameObject[] blades;
//	public GameObject fan; //a fan is a pattern of blades repeated once for ilusion of continuity
//	GameObject newBlade;
//	Quaternion rotationToReceiver;
//	private Rigidbody2D fanRigidBody;
//	public Vector3 firstBladeSetSpawnPoint;
//	public Vector3 secondBladeSetSpawnPoint;
	

//	void Update ()
//    {
//		moveFan();
//	}
	
//	public GameObject[] setupBlades(GameObjectPoolManager pools)
//	{
//		vectorToReceiver = receiver.transform.position - emitter.transform.position;
//		normalizedVectorToReceiver = vectorToReceiver.normalized;
//		normalizedVectorToEmitter = normalizedVectorToReceiver *-1f;
//		bladeLength = blade.GetComponent<BoxCollider2D>().size.x;
//		fanRigidBody = fan.GetComponent<Rigidbody2D>();
//		distanceBetweenBladeCenters = bladeLength + gap; //this is the gap + half of blade lenght on both sides

//		float angle = Mathf.Atan2(vectorToReceiver.y,vectorToReceiver.x) * Mathf.Rad2Deg;
//		rotationToReceiver = Quaternion.AngleAxis(angle, Vector3.forward);
//		numberOfBlades = (int)(vectorToReceiver.magnitude/distanceBetweenBladeCenters);

//		blades = new GameObject[numberOfBlades*2];
		
//		firstBladeSetSpawnPoint = receiver.transform.position + (normalizedVectorToEmitter * distanceBetweenBladeCenters);
//		secondBladeSetSpawnPoint = firstBladeSetSpawnPoint + normalizedVectorToEmitter * (distanceBetweenBladeCenters * numberOfBlades);
//		fan.transform.position = secondBladeSetSpawnPoint;
//		spawnBladePattern(0,firstBladeSetSpawnPoint, pools);
//		spawnBladePattern(numberOfBlades, secondBladeSetSpawnPoint, pools);
//		return blades;
//	}
	
//	public void preWarmFan(int percentOfMovementRange)
//	{
//		Vector3 offsetVector = receiver.transform.position - secondBladeSetSpawnPoint;
//		float offsetMagnitude = offsetVector.magnitude;
//		offsetMagnitude = offsetMagnitude * percentOfMovementRange * 0.01f;
//		fan.transform.position = fan.transform.position + offsetVector.normalized * offsetMagnitude;
//	}
	
//	private void spawnBladePattern(int patternOffset, Vector3 spawnPoint, GameObjectPoolManager pools)
//	{
//		for(int i =0; i< numberOfBlades ; i++)
//		{
//			newBlade = pools.retrieveObject("Blade");
			
//			newBlade.transform.position = spawnPoint + (normalizedVectorToEmitter * (distanceBetweenBladeCenters * i));
//			newBlade.transform.rotation = rotationToReceiver;             
//			blades[i+patternOffset] = newBlade; 
//			newBlade.transform.parent = fan.transform;
//		}
//	}
	
//	private void moveFan()
//	{
//		Vector2 newPosition = fan.transform.position + (normalizedVectorToReceiver * bladeSpeed * Time.deltaTime);
//		fanRigidBody.MovePosition(newPosition);
		
//		if(isBladeSegmentAtReceiver())
//		{
//			fanRigidBody.MovePosition(secondBladeSetSpawnPoint);
//		}
//	}
	
//	private bool isBladeSegmentAtReceiver()
//	{
//		if ((receiver.transform.position - fan.transform.position).magnitude <= 0.05f)
//		{
//			return true;
//		}
//		else
//		{
//			return false;
//		}
//	}
//}
