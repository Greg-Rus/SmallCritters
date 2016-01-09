using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class HeatVentFSM{

	public float totalCycleTime;
	private float[] stateStayTimes;
	private Dictionary <HeatVentState, float> stateTimers;
	private Dictionary <HeatVentState, Action<HeatVentController>> transitions;
	
	
	public HeatVentFSM()
	{
		stateTimers = new Dictionary<HeatVentState, float> ();
		stateTimers.Add (HeatVentState.Closed, 1f);
		stateTimers.Add (HeatVentState.Opening, 1f);
		stateTimers.Add (HeatVentState.WarmingUp, 1f);
		stateTimers.Add (HeatVentState.Venting, 1f);
		stateTimers.Add (HeatVentState.Closing, 1f);
		
		transitions = new Dictionary<HeatVentState, Action<HeatVentController>>();
		transitions.Add (HeatVentState.Closed, TransitionToOpening);
		transitions.Add (HeatVentState.Opening,TransitionToWarmingUp);
		transitions.Add (HeatVentState.WarmingUp, TransitionToVenting);
		transitions.Add (HeatVentState.Venting, TransitionToClosing);
		transitions.Add (HeatVentState.Closing, TransitionToClosed);
		
		calculateTotalCycleTime ();
		//stateStayTimes = new float[]{_stayCoolTime, _heatUpTime, _stayHotTime, _coolDownTime};
	}
	
	public void SetStateTimes(float[] timers)
	{
		//Debug.Log ("Count: " + stateTimers.Count);
		for(int i = 0; i < stateTimers.Count ;++i)
		{
			//Debug.Log ("Cast: " + (ProcessorState)i + " = " + timers[i]);
			stateTimers [(HeatVentState)i] = timers[i];	
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
	
	
	public void updateHeatupPhase(HeatVentController heatVent)
	{
		switch(heatVent.state)
		{
		case HeatVentState.Closed: 
			UpdateClosedState(heatVent);
			break;
			
		case HeatVentState.Opening:
			UpdateOpeningState(heatVent); 
			break;
			
		case HeatVentState.WarmingUp:
			UpdateWarmingUpState(heatVent);
			break;
			
		case HeatVentState.Venting:
			UpdateVentingState(heatVent);
			break;
			
		case HeatVentState.Closing:
			UpdateClosingState(heatVent);
			break;
		}
	}
	
	private void TransitionToClosed(HeatVentController heatVent)
	{
		SetStateAndTimer(heatVent, HeatVentState.Closed);
	}
	private void UpdateClosedState(HeatVentController heatVent)
	{
		if(Time.timeSinceLevelLoad >= heatVent.stateExitTime)
		{
			TransitionToOpening(heatVent);
		}
	}
	
	private void TransitionToOpening(HeatVentController heatVent)
	{
		SetStateAndTimer(heatVent, HeatVentState.Opening);
	}
	private void UpdateOpeningState(HeatVentController heatVent)
	{
		if(Time.timeSinceLevelLoad >= heatVent.stateExitTime)
		{
			TransitionToWarmingUp(heatVent);
		}
	}
	
	private void TransitionToWarmingUp(HeatVentController heatVent)
	{
		SetStateAndTimer(heatVent, HeatVentState.WarmingUp);
	}
	private void UpdateWarmingUpState(HeatVentController heatVent)
	{
		if(Time.timeSinceLevelLoad >= heatVent.stateExitTime)
		{
			TransitionToVenting(heatVent);
		}
	}
	
	private void TransitionToVenting(HeatVentController heatVent)
	{
		SetStateAndTimer(heatVent, HeatVentState.Venting);
	}
	private void UpdateVentingState(HeatVentController heatVent)
	{
		if(Time.timeSinceLevelLoad >= heatVent.stateExitTime)
		{
			TransitionToClosing(heatVent);
		}
	}
	
	private void TransitionToClosing(HeatVentController heatVent)
	{
		SetStateAndTimer(heatVent, HeatVentState.Closing);
	}
	private void UpdateClosingState(HeatVentController heatVent)
	{
		if(Time.timeSinceLevelLoad >= heatVent.stateExitTime)
		{
			TransitionToClosed(heatVent);
		}
	}

	private void SetStateAndTimer(HeatVentController heatVent, HeatVentState state)
	{
		heatVent.stateExitTime = Time.timeSinceLevelLoad + stateTimers[state];
		heatVent.state = state;
	}
	
	public void SetCycleCompletion(HeatVentController heatVent, float cyclePercent)
	{
		transitions[HeatVentState.Closing](heatVent);
		HeatVentState targetState = HeatVentState.Closed;
		transitions[targetState](heatVent);
		float targetStateStayTime = stateTimers[targetState];
		
		while (targetStateStayTime < totalCycleTime * cyclePercent) 
		{
			transitions[targetState](heatVent);
			++targetState;
			targetStateStayTime += stateTimers[targetState];
		}
		heatVent.state = targetState;
		heatVent.stateExitTime = Time.timeSinceLevelLoad + (targetStateStayTime - (totalCycleTime * cyclePercent));
	}
}
