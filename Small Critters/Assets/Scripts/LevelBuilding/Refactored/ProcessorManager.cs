using UnityEngine;
using System.Collections;

public class ProcessorManager : MonoBehaviour {
	SpriteRenderer mySpriteRenderer;
	public ProcessorState state;
	public float stateExitTime; //Time.timeSinceLevelLoad + some state timer
	public float stateStayTimeCompletion; // 0.0-1.0 %
	private ParticleSystem steamRing;
	
	// Use this for initialization
	void Awake () {
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		gameObject.layer = 8;
		steamRing = GetComponent<ParticleSystem>();
	}
	
	public void TintProcessorSprite(Color startColor, Color targetColor, float percent)
	{
		mySpriteRenderer.color = Color.Lerp(startColor, targetColor, percent);
	}
	public void SetProcessorSpriteColor(Color color)
	{
		mySpriteRenderer.color = color;
	}
	public void SetHazadrousLayer()
	{
		gameObject.layer = 15;
        steamRing.Simulate(0f, false, true);
		steamRing.Play();
    }
	public void SetSafeLayer()
	{
        steamRing.Pause();
        steamRing.Clear();
        steamRing.Stop();
        gameObject.layer = 8;
        
    }
}
