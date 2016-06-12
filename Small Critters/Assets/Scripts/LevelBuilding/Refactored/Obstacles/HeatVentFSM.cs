using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class HeatVentFSM{

	public float totalCycleTime;
	private Dictionary <HeatVentState, float> stateTimers;
	private Dictionary <HeatVentState, Action<HeatVentController>> transitions;
	
	
	public HeatVentFSM()
	{
		stateTimers = new Dictionary<HeatVentState, float> ();
		stateTimers.Add (HeatVentState.Closed, 1f);
		stateTimers.Add (HeatVentState.Opening, 0.4f);
		stateTimers.Add (HeatVentState.WarmingUp, 1f);
		stateTimers.Add (HeatVentState.Venting, 1.5f);
		stateTimers.Add (HeatVentState.Closing, 1f);
		
		transitions = new Dictionary<HeatVentState, Action<HeatVentController>>();
		transitions.Add (HeatVentState.Start, TransitionToClosed);
		transitions.Add (HeatVentState.Closed, TransitionToOpening);
		transitions.Add (HeatVentState.Opening,TransitionToWarmingUp);
		transitions.Add (HeatVentState.WarmingUp, TransitionToVenting);
		transitions.Add (HeatVentState.Venting, TransitionToClosing);
		transitions.Add (HeatVentState.Closing, TransitionToClosed);
		
		calculateTotalCycleTime ();
	}
	
	public void SetStateTimes(float[] timers)
	{
		for(int i = 0; i < timers.Length ;++i)
		{
			stateTimers [(HeatVentState)i+1] = timers[i];	
		}
		calculateTotalCycleTime ();
	}
	
	private void calculateTotalCycleTime()
	{
		totalCycleTime = 0;
		foreach (float time in stateTimers.Values)
		{
			totalCycleTime += time;
		}
		
	}
	
	
	public void UpdateVentingPhase(HeatVentController heatVent)
	{
		switch(heatVent.state)
		{
		case HeatVentState.Start:
			UpdateStart(heatVent);
			break;
		
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
	
	private void UpdateStart(HeatVentController heatVent)
	{
		TransitionToClosed(heatVent);
	}
	
	private void TransitionToClosed(HeatVentController heatVent)
	{
		SetStateAndTimer(heatVent, HeatVentState.Closed);
		UpdateClosedState(heatVent);
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
		UpdateOpeningState(heatVent);
	}
	private void UpdateOpeningState(HeatVentController heatVent)
	{
		if(Time.timeSinceLevelLoad >= heatVent.stateExitTime)
		{
			TransitionToWarmingUp(heatVent);
		}
		else
		{
			heatVent.UpdateVentDoorOpening();
		}
		  
	}
	
	private void TransitionToWarmingUp(HeatVentController heatVent)
	{
		SetStateAndTimer(heatVent, HeatVentState.WarmingUp);
		UpdateWarmingUpState(heatVent);
	}
	private void UpdateWarmingUpState(HeatVentController heatVent)
	{
		if(Time.timeSinceLevelLoad >= heatVent.stateExitTime)
		{
			TransitionToVenting(heatVent);
		}
		else
		{
			heatVent.UpdateWarmingUp();
		}
	}
	
	private void TransitionToVenting(HeatVentController heatVent)
	{
		SetStateAndTimer(heatVent, HeatVentState.Venting);
		heatVent.SetHazadrousLayer();
		UpdateVentingState(heatVent);
	}
	private void UpdateVentingState(HeatVentController heatVent)
	{
		if(Time.timeSinceLevelLoad >= heatVent.stateExitTime)
		{
			TransitionToClosing(heatVent);
		}
		else
		{
			heatVent.UpdateVentigState();	
		}
		
	}
	
	private void TransitionToClosing(HeatVentController heatVent)
	{
		SetStateAndTimer(heatVent, HeatVentState.Closing);
		heatVent.SetSafeLayer();
		UpdateClosingState(heatVent);
	}
	private void UpdateClosingState(HeatVentController heatVent)
	{
		if(Time.timeSinceLevelLoad >= heatVent.stateExitTime)
		{
			TransitionToClosed(heatVent);
		}
		else
		{
			heatVent.UpdateVentDoorClosing();
			
		}
	}

	private void SetStateAndTimer(HeatVentController heatVent, HeatVentState state)
	{
		heatVent.StateExitTime = Time.timeSinceLevelLoad + stateTimers[state];
		heatVent.state = state;
	}
	
	public void SetCycleCompletion(HeatVentController heatVent, float cyclePercent)
	{
		transitions[HeatVentState.Start](heatVent);
		HeatVentState targetState = HeatVentState.Closed;
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
