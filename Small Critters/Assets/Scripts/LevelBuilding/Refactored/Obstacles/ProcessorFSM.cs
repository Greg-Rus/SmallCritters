using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ProcessorFSM: IProcessorFSM{
	

	public Color maxHeatupColor = Color.red;
	public float totalCycleTime;
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
		transitions.Add (ProcessorState.Cool, TransitionToHeatingUp);
		transitions.Add (ProcessorState.HeatingUp, TransitionToHot);
		transitions.Add (ProcessorState.Hot, TransitionToCoolingDown);
		transitions.Add (ProcessorState.CoolingDown, TransitionToCool);
		
		CalculateTotalCycleTime ();
	}

	public void SetStateTimes(float[] timers)
	{
		for(int i = 0; i < stateTimers.Count ;++i)
		{
			stateTimers [(ProcessorState)i] = timers[i];	
		}
		CalculateTotalCycleTime ();
	}
	
	private void CalculateTotalCycleTime()
	{
		totalCycleTime = 0;
		foreach (float time in stateTimers.Values)
		{
			totalCycleTime += time;
		}
			
	}
	
	public void UpdateHeatupPhase(ProcessorManager processor)
	{
		switch(processor.state)
		{
		case ProcessorState.Cool: 
			StayCool(processor);
			break;
			
		case ProcessorState.HeatingUp:
			HeatUp(processor); 
			break;
			
		case ProcessorState.Hot:
			StayHot(processor);
			break;
			
		case ProcessorState.CoolingDown:
			CoolDown(processor);
			break;
		}
	}
	
	private void StayCool(ProcessorManager processor)
	{
		if(Time.timeSinceLevelLoad >= processor.stateExitTime)
		{
			TransitionToHeatingUp(processor);
		}
	}
	
	private void TransitionToHeatingUp(ProcessorManager processor)
	{
		SetStateAndTimer(processor, ProcessorState.HeatingUp);
	}
	
	private void HeatUp(ProcessorManager processor)
	{
		float heatUpPercent = TimerProgressPercent(processor, stateTimers[ProcessorState.HeatingUp]);
		processor.TintProcessorSprite(Color.white, maxHeatupColor, heatUpPercent);
		if(heatUpPercent >= 0.99f)
		{
			processor.SetProcessorSpriteColor(Color.red);
			TransitionToHot(processor);
		}
	}
	
	private void TransitionToHot(ProcessorManager processor)
	{
		processor.SetHazadrousLayer();
		SetStateAndTimer(processor, ProcessorState.Hot);
	}
	
	private void StayHot(ProcessorManager processor)
	{
		if(Time.timeSinceLevelLoad >= processor.stateExitTime)
		{
			TransitionToCoolingDown(processor);
		}
	}
	private void TransitionToCoolingDown(ProcessorManager processor)
	{
		processor.SetSafeLayer();
		SetStateAndTimer(processor, ProcessorState.CoolingDown);
	}
	
	private void CoolDown(ProcessorManager processor)
	{
		float coolDownPercent = TimerProgressPercent(processor, stateTimers[ProcessorState.CoolingDown]);
		processor.TintProcessorSprite(maxHeatupColor, Color.white, coolDownPercent);
		if(coolDownPercent >= 0.99f)
		{
			processor.SetProcessorSpriteColor(Color.white);
			TransitionToCool(processor);
		}
	}
	private void TransitionToCool(ProcessorManager processor)
	{
		SetStateAndTimer(processor, ProcessorState.Cool);
	}
	
	private float TimerProgressPercent(ProcessorManager processor, float stateStayTime)
	{
		float timeRemaining = processor.stateExitTime - Time.timeSinceLevelLoad;
		float timeElapsed = stateStayTime - timeRemaining;
		return timeElapsed / stateStayTime;
	}
	
	public void SetCycleCompletion(ProcessorManager processor, float cyclePercent)
	{
		transitions[ProcessorState.CoolingDown](processor);
		ProcessorState targetState = ProcessorState.Cool;

		float targetStateStayTime = stateTimers[targetState];
		
		while (targetStateStayTime < totalCycleTime * cyclePercent) 
		{
			transitions[targetState](processor);
			++targetState;
			
			targetStateStayTime += stateTimers[targetState];
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
