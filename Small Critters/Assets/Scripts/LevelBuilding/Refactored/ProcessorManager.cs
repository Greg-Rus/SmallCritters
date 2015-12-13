using UnityEngine;
using System.Collections;

public class ProcessorManager : MonoBehaviour {
	SpriteRenderer mySpriteRenderer;
	public ProcessorState state;
	public float cycleTime;
	
	// Use this for initialization
	void Start () {
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		gameObject.layer = 8;
	}
	
	public void colorProcessorSprite(Color startColor, Color targetColor, float percent)
	{
		mySpriteRenderer.color = Color.Lerp(startColor, targetColor, percent);
	}
	
	void OnTriggerStay2D(Collider2D other) {
		if(state == ProcessorState.Hot && other.tag == "Player")
		{
			other.GetComponent<FrogController>().die(); //TODO should be an event
		}
	}
}
