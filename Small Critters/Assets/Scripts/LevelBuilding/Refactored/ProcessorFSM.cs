using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProcessorFSM: IProcessorFSM{
	
	//private float _heatUpTime = 1;
	//private float _coolDownTime = 1;
	//private float _stayHotTime = 1;
	//private float _stayCoolTime = 1;
/*	public float heatUpTime
	{
		get{return this._heatUpTime;}
		set{this._heatUpTime = value; calculateTotalCycleTime();}
	}
	public float coolDownTime
	{
		get{return this._coolDownTime;}
		set{this._coolDownTime = value; calculateTotalCycleTime();}
	}
	public float stayHotTime
	{
		get{return this._stayHotTime;}
		set{this._stayHotTime = value; calculateTotalCycleTime();}
	}
	public float stayCoolTime
	{
		get{return this._stayCoolTime;}
		set{this._stayCoolTime = value; calculateTotalCycleTime();}
	}
	*/
	public Color maxHeatupColor = Color.red;
	public float totalCycleTime;
	private float[] stateStayTimes;
	private Dictionary<ProcessorState, float> stateTimers;

	
	public ProcessorFSM()
	{
		stateTimers = new Dictionary<ProcessorState, float> ();
		stateTimers.Add (ProcessorState.Cool, 1f);
		stateTimers.Add (ProcessorState.HeatingUp, 1f);
		stateTimers.Add (ProcessorState.Hot, 1f);
		stateTimers.Add (ProcessorState.CoolingDown, 1f);

		calculateTotalCycleTime ();
		//stateStayTimes = new float[]{_stayCoolTime, _heatUpTime, _stayHotTime, _coolDownTime};
	}

	public void changeStateTimer(ProcessorState state, float time)
	{
		stateTimers [state] = time;
		calculateTotalCycleTime ();
	}
	
	private void calculateTotalCycleTime()
	{
		foreach (float time in stateTimers.Values)
			totalCycleTime += time;
	}
	
	
	public void updateHeatupPhase(ProcessorManager processor)
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
		processor.stateExitTime = Time.timeSinceLevelLoad + stateTimers[ProcessorState.HeatingUp];
		processor.state = ProcessorState.HeatingUp;
	}
	
	private void heatUp(ProcessorManager processor)
	{
		float heatUpPercent = timerProgressPercent(processor, stateTimers[ProcessorState.HeatingUp]);
		processor.tintProcessorSprite(Color.white, maxHeatupColor, heatUpPercent);
		if(heatUpPercent >= 0.95)
		{
			processor.setProcessorSpriteColor(Color.red);
			transitionToHot(processor);
		}
	}
	
	private void transitionToHot(ProcessorManager processor)
	{
		processor.stateExitTime = Time.timeSinceLevelLoad + stateTimers[ProcessorState.Hot];
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
		processor.stateExitTime = Time.timeSinceLevelLoad + stateTimers[ProcessorState.CoolingDown];
		processor.setSafeLayer();
		processor.state = ProcessorState.CoolingDown;
	}
	
	private void coolDown(ProcessorManager processor)
	{
		float coolDownPercent = timerProgressPercent(processor, stateTimers[ProcessorState.CoolingDown]);
		processor.tintProcessorSprite(maxHeatupColor, Color.white, coolDownPercent);
		if(coolDownPercent >= 0.95)
		{
			processor.setProcessorSpriteColor(Color.white);
			transitionToCool(processor);
		}
	}
	private void transitionToCool(ProcessorManager processor)
	{
		processor.stateExitTime = Time.timeSinceLevelLoad + stateTimers[ProcessorState.Cool];
		processor.state = ProcessorState.Cool;
	}
	
	private float timerProgressPercent(ProcessorManager processor, float stateStayTime)
	{
		float timeRemaining = processor.stateExitTime - Time.timeSinceLevelLoad;
		float timeElapsed = stateStayTime - timeRemaining;
		return timeElapsed / stateStayTime;
	}
	
	public void setCycleCompletion(ProcessorManager processor, float cyclePercent)
	{
		ProcessorState targetState = ProcessorState.Cool;
		//int timesIndex = 0;
		float targetStateStayTime = stateTimers[targetState];
		
		while (targetStateStayTime < totalCycleTime * cyclePercent) 
		{
			Debug.Log (targetStateStayTime + "  " + totalCycleTime * cyclePercent);
			++targetState;
			targetStateStayTime += stateTimers[targetState];
			//++timesIndex;
		}
		processor.state = targetState;
		processor.stateExitTime = Time.timeSinceLevelLoad + (targetStateStayTime - (totalCycleTime * cyclePercent));
	}
}
