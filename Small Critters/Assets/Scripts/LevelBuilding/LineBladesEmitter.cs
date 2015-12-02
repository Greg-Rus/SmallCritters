using UnityEngine;
using System.Collections;

public class LineBladesEmitter : MonoBehaviour {
	public GameObject blade;
	public GameObject terminator;
	public float gap;
	public float bladeSpeed;
	private GameObject[] blades;
	private int numberOfBlades;
	private Vector3 normalizedVectorToReceiver;
	private Vector3 vectorToReceiver;
	public float distanceToReciever;
	public float bladeLength;
	public float distanceBetweenBladeCenters;
	public float distanceToTerminatorEdge;
	public float distanceToTerminationRadius;
	public float terminationRadius;

	// Use this for initialization
	void Start () {
		setupBlades();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		moveBlades();
	}
	
	private void setupBlades()
	{
		vectorToReceiver = terminator.transform.position - this.transform.position;
		normalizedVectorToReceiver = vectorToReceiver.normalized;
		distanceToReciever = vectorToReceiver.magnitude;
		distanceToTerminatorEdge = distanceToReciever- terminator.GetComponent<CircleCollider2D>().radius;
		//terminationDistance = distanceToTerminatorEdge- bladeLength *0.5f;
		distanceToTerminationRadius = distanceToReciever - terminationRadius;
		bladeLength = blade.GetComponent<BoxCollider2D>().size.x;
		terminationRadius = terminator.GetComponent<CircleCollider2D>().radius + bladeLength *0.5f;
		
		distanceBetweenBladeCenters = bladeLength + gap; //this is the gap + half of blade lenght on both sides
		

		
		Vector3 dir = terminator.transform.position - this.transform.position;
		float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
		Quaternion rotationToTerminator = Quaternion.AngleAxis(angle, Vector3.forward);
		
		numberOfBlades = (int)(distanceToTerminationRadius/distanceBetweenBladeCenters);
		Debug.Log("Spawning " + numberOfBlades + " blades at distance: " + distanceToTerminationRadius);
		//Debug.Log(distanceBetweenBladeCenters);
		offsetTerminator();

		blades = new GameObject[numberOfBlades];
		
		for(int i =0; i< numberOfBlades ; i++)
		{
			GameObject newBlade = Instantiate(blade, 
			                        this.transform.position +( normalizedVectorToReceiver * distanceBetweenBladeCenters * i),
			                                  rotationToTerminator) as GameObject;                    
			blades[i] = newBlade;
		}
	}
	
	private void moveBlades()
	{
		for( int i = 0 ; i < numberOfBlades ; i++)
		{
			moveBlade(blades[i]);
		}
	}
	
	private void moveBlade(GameObject blade)
	{
		Vector2 newPosition = blade.transform.position + (normalizedVectorToReceiver * bladeSpeed * Time.deltaTime);
		blade.GetComponent<Rigidbody2D>().MovePosition(newPosition);
	}
	
	public bool isOwnBlade(GameObject objectAtTerminator)
	{
		for( int i = 0 ; i < numberOfBlades ; i++)
		{
			if(blades[i] == objectAtTerminator)
			{
				return true;
			}
		}
		return false;
	}
	
	private void offsetTerminator()
	{
		if(distanceToTerminationRadius > distanceBetweenBladeCenters * numberOfBlades)
		{
			float desireddistanceToTerminationRadius = distanceBetweenBladeCenters * (numberOfBlades+1);
			float offset = desireddistanceToTerminationRadius - distanceToReciever + terminationRadius;
			Debug.Log(offset);
			terminator.transform.position = terminator.transform.position + normalizedVectorToReceiver * offset;
			numberOfBlades++;
		}
	}
}
