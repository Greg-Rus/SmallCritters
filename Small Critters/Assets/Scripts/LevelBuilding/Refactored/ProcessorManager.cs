using UnityEngine;
using System.Collections;

public class ProcessorManager : MonoBehaviour {
	SpriteRenderer mySpriteRenderer;
	public ProcessorState state;
	//public float cycleTime;
	public float stateExitTime; //Time.timeSinceLevelLoad + some state timer
	public float stateStayTimeCompletion; // 0.0-1.0 %
	private ParticleSystem steamRing;
	
	// Use this for initialization
	void Start () {
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		gameObject.layer = 8;
		steamRing = GetComponent<ParticleSystem>();
	}

	
	public void tintProcessorSprite(Color startColor, Color targetColor, float percent)
	{
		mySpriteRenderer.color = Color.Lerp(startColor, targetColor, percent);
	}
	public void setProcessorSpriteColor(Color color)
	{
		mySpriteRenderer.color = color;
	}
	public void setHazadrousLayer()
	{
		gameObject.layer = 15;
		steamRing.Play();
		//StartSteamParticles();
	}
	public void setSafeLayer()
	{
		gameObject.layer = 8;
		steamRing.Stop();
	}
	
	void OnTriggerStay2D(Collider2D other) {
		if(state == ProcessorState.Hot && other.tag == "Player")
		{
			other.GetComponent<FrogController>().die();
		}
	}
}
