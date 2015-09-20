using UnityEngine;
using System.Collections;

public class ProcessorHeater : MonoBehaviour {
	public enum ProcessorState {Cool, HeatingUp, Hot, CoolingDown};
	SpriteRenderer mySpriteRenderer;
	public float heatUpTime;
	public float coolDownTime;
	public float stayHotTime;
	public float stayCoolTime;
	public ProcessorState state = ProcessorState.Cool;
	private float expireTime;
	private BoxCollider2D myBoxCollider;
	private float[] timers;
	public Color maxHeatupColor;
	// Use this for initialization
	void Awake () {
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		gameObject.layer = 8;
		timers = new float[]{stayCoolTime, heatUpTime, stayHotTime, coolDownTime};
	}
	
	// Update is called once per frame
	void Update () {
		updateHeatupPhase();
	}
	
	private void updateHeatupPhase()
	{
		switch(state)
		{
			case ProcessorState.Cool: 
			stayCool();
			break;
			
			case ProcessorState.HeatingUp:
			heatUp(); 
			break;
			
			case ProcessorState.Hot:
			stayHot();
			break;
			
			case ProcessorState.CoolingDown:
			coolDown();
			break;
			
		}
	}
	private void stayCool()
	{
		if(Time.timeSinceLevelLoad >= expireTime)
		{
			transitionToHeatingUp();
		}
	}
	private void heatUp()
	{
		//float remainingHeatUpTime = expireTime - Time.timeSinceLevelLoad;
		//float heatUpTimeElapsed = heatUpTime - remainingHeatUpTime;
		float heatUpPercent = timerProgressPercent(heatUpTime);
		colorProcessorSprite(Color.white, maxHeatupColor, heatUpPercent);
		if(heatUpPercent >= 0.95)
		{
			mySpriteRenderer.color = Color.red;
			transitionToHot();
		}
	}
	
	private void stayHot()
	{
		if(Time.timeSinceLevelLoad >= expireTime)
		{
			transitionToCoolingDown();
		}
	}

	private void coolDown()
	{
		float coolDownPercent = timerProgressPercent(coolDownTime);
		colorProcessorSprite(maxHeatupColor, Color.white, coolDownPercent);
		if(coolDownPercent >= 0.95)
		{
			mySpriteRenderer.color = Color.white;
			transitionToCool();
		}
	}

	private void transitionToHeatingUp()
	{
		expireTime = Time.timeSinceLevelLoad + heatUpTime;
		state++;
	}
	
	private void transitionToHot()
	{
		expireTime = Time.timeSinceLevelLoad + stayHotTime;
		gameObject.layer = 15;
		state++;
	}
	
	private void transitionToCoolingDown()
	{
		expireTime = Time.timeSinceLevelLoad + coolDownTime;
		gameObject.layer = 8;
		state++;
	}
	
	private void transitionToCool()
	{
		expireTime = Time.timeSinceLevelLoad + stayCoolTime;
		state = ProcessorState.Cool;
	}
	
	private void colorProcessorSprite(Color startColor, Color targetColor, float percent)
	{
		mySpriteRenderer.color = Color.Lerp(startColor, targetColor, percent);
	}
	
	private float timerProgressPercent(float time)
	{
		float remainingTime = expireTime - Time.timeSinceLevelLoad;
		float timeElapsed = time - remainingTime;
		return timeElapsed / time;
	}
	void OnTriggerStay2D(Collider2D other) {
		if(state == ProcessorState.Hot && other.tag == "Player")
		{
			other.GetComponent<FrogController>().die();
		}

	}
	public void setProcessorState(float cycleProgressPercent)
	{
		float totalCycleTime = 0;
		foreach (float timer in timers)
		{
			totalCycleTime += timer;
		}
		float targetCycleTime = totalCycleTime * cycleProgressPercent;
		Debug.Log ("TargetCycleTime: " + targetCycleTime);
		float skippedTime =0;
		ProcessorState targetState = ProcessorState.Cool;
		foreach(float timer in timers)
		{
			skippedTime += timer;
			if (skippedTime >= targetCycleTime)
			{
				break;
			}
			targetState++;
		}
		float targetStateTime = skippedTime - targetCycleTime;

		
		this.state = targetState;
		expireTime = Time.timeSinceLevelLoad + targetStateTime;
		Debug.Log ("State: " + targetState + " time left: " + targetStateTime);
	}
}
