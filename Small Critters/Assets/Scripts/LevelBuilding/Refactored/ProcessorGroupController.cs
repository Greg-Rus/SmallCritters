using UnityEngine;
using System.Collections;

public class ProcessorGroupController : MonoBehaviour {
	public ProcessorManager[,] processorGroup;
	int patternVariant;
	public float heatUpTime;
	public float coolDownTime;
	public float stayHotTime;
	public float stayCoolTime;
	public Color maxHeatupColor;
	// Use this for initialization
	void Start () {
	
	}
	
	public void initialize(ProcessorManager[,] processorGroup, int patternVariant)
	{
		this.processorGroup = processorGroup;
		repartentProcessors();
		processorGroupInitialSetup(patternVariant);
	}
	
	// Update is called once per frame
	void Update () {
		foreach(ProcessorManager processor in processorGroup)
		{
			updateHeatupPhase(processor);
		}
	}
	
	private void updateHeatupPhase(ProcessorManager processor)
	{
		
		switch(processor.state)
		{
		case ProcessorState.Cool: 
			stayCool(processor);
			break;
			
		case ProcessorState.HeatingUp:
			heatUp(processor); 
			break;
			
		case ProcessorState.Hot:
			stayHot(processor);
			break;
			
		case ProcessorState.CoolingDown:
			coolDown(processor);
			break;
			
		}

	}
	
	private void stayCool(ProcessorManager processor)
	{
		if(Time.timeSinceLevelLoad >= processor.stateExitTime)
		{
			transitionToHeatingUp(processor);
		}
	}
	
	private void transitionToHeatingUp(ProcessorManager processor)
	{
		processor.stateExitTime = Time.timeSinceLevelLoad + heatUpTime;
		processor.state = ProcessorState.HeatingUp;
	}
	
	private void heatUp(ProcessorManager processor)
	{
		float heatUpPercent = timerProgressPercent(processor, heatUpTime);
		processor.tintProcessorSprite(Color.white, maxHeatupColor, heatUpPercent);
		if(heatUpPercent >= 0.95)
		{
			processor.setProcessorSpriteColor(Color.red);
			transitionToHot(processor);
		}
	}
	
	private void transitionToHot(ProcessorManager processor)
	{
		processor.stateExitTime = Time.timeSinceLevelLoad + stayHotTime;
		processor.setHazadrousLayer();
		processor.state = ProcessorState.Hot;
	}
	
	private void stayHot(ProcessorManager processor)
	{
		if(Time.timeSinceLevelLoad >= processor.stateExitTime)
		{
			transitionToCoolingDown(processor);
		}
	}
	private void transitionToCoolingDown(ProcessorManager processor)
	{
		processor.stateExitTime = Time.timeSinceLevelLoad + coolDownTime;
		processor.setSafeLayer();
		processor.state = ProcessorState.CoolingDown;
	}
	
	private void coolDown(ProcessorManager processor)
	{
		float coolDownPercent = timerProgressPercent(processor, coolDownTime);
		processor.tintProcessorSprite(maxHeatupColor, Color.white, coolDownPercent);
		if(coolDownPercent >= 0.95)
		{
			processor.setProcessorSpriteColor(Color.white);
			transitionToCool(processor);
		}
	}
	private void transitionToCool(ProcessorManager processor)
	{
		processor.stateExitTime = Time.timeSinceLevelLoad + stayCoolTime;
		processor.state = ProcessorState.Cool;
	}
	
	private float timerProgressPercent(ProcessorManager processor, float stateStayTime)
	{
		float timeRemaining = processor.stateExitTime - Time.timeSinceLevelLoad;
		float timeElapsed = stateStayTime - timeRemaining;
		return timeElapsed / stateStayTime;
	}
	
	private void processorGroupInitialSetup(int patternVariant)
	{
		Debug.Log ("Initial Setup for pattern number " + patternVariant);
	}
	
	private void repartentProcessors()
	{
		foreach(ProcessorManager processor in processorGroup)
		{
			processor.transform.parent = this.transform;
		}
	}
}
