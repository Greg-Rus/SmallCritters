﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ProcessorFSM: IProcessorFSM{
	

	public Color maxHeatupColor = Color.red;
	public float totalCycleTime;
	private float[] stateStayTimes;
	private Dictionary<ProcessorState, float> stateTimers;
	private Dictionary <ProcessorState, Action<ProcessorManager>> transitions;

	
	public ProcessorFSM()
	{
		stateTimers = new Dictionary<ProcessorState, float> ();
		stateTimers.Add (ProcessorState.Cool, 1f);
		stateTimers.Add (ProcessorState.HeatingUp, 1f);
		stateTimers.Add (ProcessorState.Hot, 1f);
		stateTimers.Add (ProcessorState.CoolingDown, 1f);
		
		transitions = new Dictionary<ProcessorState, Action<ProcessorManager>>();
		transitions.Add (ProcessorState.Cool, transitionToHeatingUp);
		transitions.Add (ProcessorState.HeatingUp, transitionToHot);
		transitions.Add (ProcessorState.Hot, transitionToCoolingDown);
		transitions.Add (ProcessorState.CoolingDown, transitionToCool);
		

		calculateTotalCycleTime ();
		//stateStayTimes = new float[]{_stayCoolTime, _heatUpTime, _stayHotTime, _coolDownTime};
	}

	public void SetStateTimes(float[] timers)
	{
		//Debug.Log ("Count: " + stateTimers.Count);
		for(int i = 0; i < stateTimers.Count ;++i)
		{
			//Debug.Log ("Cast: " + (ProcessorState)i + " = " + timers[i]);
			stateTimers [(ProcessorState)i] = timers[i];	
		}
		calculateTotalCycleTime ();
	}
	
	private void calculateTotalCycleTime()
	{
		totalCycleTime = 0;
		foreach (float time in stateTimers.Values)
		{
			totalCycleTime += time;
			//Debug.Log ("Time: " + time + " total: " + totalCycleTime);
		}
			
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
		SetStateAndTimer(processor, ProcessorState.HeatingUp);
		//processor.stateExitTime = Time.timeSinceLevelLoad + stateTimers[ProcessorState.HeatingUp];
		//processor.state = ProcessorState.HeatingUp;
	}
	
	private void heatUp(ProcessorManager processor)
	{
		float heatUpPercent = timerProgressPercent(processor, stateTimers[ProcessorState.HeatingUp]);
		processor.tintProcessorSprite(Color.white, maxHeatupColor, heatUpPercent);
		if(heatUpPercent >= 0.99f)
		{
			processor.setProcessorSpriteColor(Color.red);
			transitionToHot(processor);
		}
	}
	
	private void transitionToHot(ProcessorManager processor)
	{
		processor.setHazadrousLayer();
		SetStateAndTimer(processor, ProcessorState.Hot);
		//processor.stateExitTime = Time.timeSinceLevelLoad + stateTimers[ProcessorState.Hot];
		
		//processor.state = ProcessorState.Hot;
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
		processor.setSafeLayer();
		SetStateAndTimer(processor, ProcessorState.CoolingDown);
		//processor.stateExitTime = Time.timeSinceLevelLoad + stateTimers[ProcessorState.CoolingDown];
		
		//processor.state = ProcessorState.CoolingDown;
	}
	
	private void coolDown(ProcessorManager processor)
	{
		float coolDownPercent = timerProgressPercent(processor, stateTimers[ProcessorState.CoolingDown]);
		processor.tintProcessorSprite(maxHeatupColor, Color.white, coolDownPercent);
		if(coolDownPercent >= 0.99f)
		{
			processor.setProcessorSpriteColor(Color.white);
			transitionToCool(processor);
		}
	}
	private void transitionToCool(ProcessorManager processor)
	{
		SetStateAndTimer(processor, ProcessorState.Cool);
		//processor.stateExitTime = Time.timeSinceLevelLoad + stateTimers[ProcessorState.Cool];
		//processor.state = ProcessorState.Cool;
	}
	
	private float timerProgressPercent(ProcessorManager processor, float stateStayTime)
	{
		float timeRemaining = processor.stateExitTime - Time.timeSinceLevelLoad;
		float timeElapsed = stateStayTime - timeRemaining;
		return timeElapsed / stateStayTime;
	}
	
	public void SetCycleCompletion(ProcessorManager processor, float cyclePercent)
	{
		transitions[ProcessorState.CoolingDown](processor);
		ProcessorState targetState = ProcessorState.Cool;
		//int timesIndex = 0;
		float targetStateStayTime = stateTimers[targetState];
		
		while (targetStateStayTime < totalCycleTime * cyclePercent) 
		{
			//Debug.Log (targetStateStayTime + "  " + totalCycleTime * cyclePercent);
			transitions[targetState](processor);
			++targetState;
			
			targetStateStayTime += stateTimers[targetState];
			//++timesIndex;
		}
		processor.state = targetState;
		processor.stateExitTime = Time.timeSinceLevelLoad + (targetStateStayTime - (totalCycleTime * cyclePercent));
	}
	
	private void SetStateAndTimer(ProcessorManager processor, ProcessorState state)
	{
		processor.stateExitTime = Time.timeSinceLevelLoad + stateTimers[state];
		processor.state = state;
	}
}
