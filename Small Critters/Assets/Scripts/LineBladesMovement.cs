using UnityEngine;
using System.Collections;

public class LineBladesMovement : MonoBehaviour {
	public GameObject blade;
	public GameObject emitter;
	public GameObject receiver;
	public float gap;
	public float bladeSpeed;
	private Vector3 normalizedVectorToReceiver;
	private Vector3 normalizedVectorToEmitter;
	private Vector3 vectorToReceiver;
	private float distanceToReciever;
	private float bladeLength;
	public int numberOfBlades;
	private float distanceBetweenBladeCenters;
	private GameObject[] blades;
	public GameObject fan; //a fan is a pattern of blades repeated once for ilusion of continuity
	GameObject newBlade;
	Quaternion rotationToReceiver;
	private Rigidbody2D fanRigidBody;
	Vector3 firstBladeSetSpawnPoint;
	Vector3 secondBladeSetSpawnPoint;
	// Use this for initialization
	void Start () {
		setupBlades();
	}
	
	// Update is called once per frame
	void Update () {
		moveFan();
	}
	
	private void setupBlades()
	{
		vectorToReceiver = receiver.transform.position - emitter.transform.position;
		normalizedVectorToReceiver = vectorToReceiver.normalized;
		normalizedVectorToEmitter = normalizedVectorToReceiver *-1f;
		distanceToReciever = vectorToReceiver.magnitude;
		bladeLength = blade.GetComponent<BoxCollider2D>().size.x;
		fanRigidBody = fan.GetComponent<Rigidbody2D>();
	
		distanceBetweenBladeCenters = bladeLength + gap; //this is the gap + half of blade lenght on both sides

		float angle = Mathf.Atan2(vectorToReceiver.y,vectorToReceiver.x) * Mathf.Rad2Deg;
		rotationToReceiver = Quaternion.AngleAxis(angle, Vector3.forward);
		
		numberOfBlades = (int)(vectorToReceiver.magnitude/distanceBetweenBladeCenters);
		
		
		
		blades = new GameObject[numberOfBlades*2];
		
		firstBladeSetSpawnPoint = receiver.transform.position + (normalizedVectorToEmitter * distanceBetweenBladeCenters);// + (normalizedVectorToEmitter * (receiver.GetComponent<CircleCollider2D>().radius + bladeLength * 0.5f));
		secondBladeSetSpawnPoint = firstBladeSetSpawnPoint + normalizedVectorToEmitter * (distanceBetweenBladeCenters * numberOfBlades);
		fan.transform.position = secondBladeSetSpawnPoint;
		spawnBladePattern(0,firstBladeSetSpawnPoint);
		spawnBladePattern(numberOfBlades, secondBladeSetSpawnPoint);
	}
	
	private void spawnBladePattern(int patternOffset, Vector3 spawnPoint)
	{
		for(int i =0; i< numberOfBlades ; i++)
		{
			newBlade = Instantiate(blade, 
			                       spawnPoint +( normalizedVectorToEmitter * (distanceBetweenBladeCenters * i)),
			                       rotationToReceiver) as GameObject;                    
			blades[i+patternOffset] = newBlade; 
			newBlade.transform.parent = fan.transform;
		}
	}
	
	private void moveFan()
	{
		Vector2 newPosition = fan.transform.position + (normalizedVectorToReceiver * bladeSpeed * Time.deltaTime);
		fanRigidBody.MovePosition(newPosition);
		
		if(isBladeSegmentAtReceiver())
		{
			fanRigidBody.MovePosition(secondBladeSetSpawnPoint);
			Debug.Log("Reset");
		}
	}
	
	private bool isBladeSegmentAtReceiver()
	{
		if ((receiver.transform.position - fan.transform.position).magnitude <= 0.05f)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
