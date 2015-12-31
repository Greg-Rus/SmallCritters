using UnityEngine;
using System.Collections;
using System;

public class ProcessorGroupController : MonoBehaviour {
	public ProcessorManager[,] processorGroup;
	int patternVariant;
	public IProcessorFSM processorStateMachine;

	//TODO separate class
	int oldI = 0;
	int oldJ = 0;
	float totalCycleOffset = 0;

	// Use this for initialization
	void Awake () 
	{
		if (processorStateMachine == null) 
			{
			processorStateMachine = ServiceLocator.getService<IProcessorFSM>();
			}
	}
	
	public void initialize(ProcessorManager[,] processorGroup, int patternVariant)
	{
		this.processorGroup = processorGroup;
		repartentProcessors();
		processorGroupInitialSetup(patternVariant);
	}
	
	// Update is called once per frame
	void Update () 
	{
		foreach(ProcessorManager processor in processorGroup)
		{
			processorStateMachine.updateHeatupPhase(processor);
		}
	}
	

	
	private void processorGroupInitialSetup(int patternVariant)
	{
		//Debug.Log ("Initial Setup for pattern number " + patternVariant);

		if (patternVariant == 1) //Right to left
		{
			float cycleOffset = 1f / processorGroup.GetLength(0);
			float processorColumnOffset = 0f;
			for(int i = 0 ; i < processorGroup.GetLength(0) ;++i)
			{
				for(int j = 0 ; j < processorGroup.GetLength(1) ;++j)
				{
					processorStateMachine.setCycleCompletion(processorGroup[i,j],processorColumnOffset);
				}
				processorColumnOffset += cycleOffset;
			}
		}
		if (patternVariant == 2) //Left to right
		{
			float cycleOffset = 1f / processorGroup.GetLength(0);
			float processorColumnOffset = 1f;
			for(int i = 0 ; i < processorGroup.GetLength(0) ;++i)
			{
				for(int j = 0 ; j < processorGroup.GetLength(1) ;++j)
				{
					processorStateMachine.setCycleCompletion(processorGroup[i,j],processorColumnOffset);
				}
				processorColumnOffset -= cycleOffset;
			}
		}
		if (patternVariant == 3) //top to down
		{
			float cycleOffset = 1f / processorGroup.GetLength(0);
			float processorColumnOffset = 0f;
			for(int i = 0 ; i < processorGroup.GetLength(0) ;++i)
			{
				for(int j = 0 ; j < processorGroup.GetLength(1) ;++j)
				{
					processorStateMachine.setCycleCompletion(processorGroup[i,j],processorColumnOffset);
					processorColumnOffset += cycleOffset;
				}
				processorColumnOffset = 0f;
			}
		}
		if (patternVariant == 4) //top to down
		{
			float cycleOffset = 1f / processorGroup.GetLength(0);
			float processorColumnOffset = 1f;
			for(int i = 0 ; i < processorGroup.GetLength(0) ;++i)
			{
				for(int j = 0 ; j < processorGroup.GetLength(1) ;++j)
				{
					processorStateMachine.setCycleCompletion(processorGroup[i,j],processorColumnOffset);
					processorColumnOffset -= cycleOffset;
				}
				processorColumnOffset = 1f;
			}
		}
		if (patternVariant == 5) //Checkered
		{
			//float cycleOffset = 1f / processorGroup.GetLength(0);
			float processorColumnOffset = 0f;
			for(int i = 0 ; i < processorGroup.GetLength(0) ;++i)
			{
				for(int j = 0 ; j < processorGroup.GetLength(1) ;++j)
				{
					processorColumnOffset = ((j+i) % 2 == 0) ? 0f : 0.5f;
					processorStateMachine.setCycleCompletion(processorGroup[i,j],processorColumnOffset);
				}
			}
		}
		if (patternVariant == 6)
		{
			DeployPatternToProcessorGroup(TopDown);
		}
	}

	private void DeployPatternToProcessorGroup(Func<int,int,float> columnOffset)
	{
		for(int i = 0 ; i < processorGroup.GetLength(0) ;++i)
		{
			for(int j = 0 ; j < processorGroup.GetLength(1) ;++j)
			{
				processorStateMachine.setCycleCompletion(processorGroup[i,j],columnOffset(i,j));
			}
		}
	}

	private float GetCycleOffset()
	{
		return 1f / processorGroup.GetLength(0);
	}
	
	private float GetCycleOffsetY()
	{
		return 1f / processorGroup.GetLength(1);
	}

	private float TopDown(int i, int j)
	{
		if(i==0 & j == 0)
		{
			totalCycleOffset = 1f;
		}
		if(i != oldI)
		{
			//Debug.Log("cycleOffsetX * i: " + (1f - GetCycleOffset() * i));
			totalCycleOffset = 1f - (GetCycleOffset() * i) + GetCycleOffset();			
			oldI = i;
		} 
		if (j != oldJ) 
		{
			totalCycleOffset -= GetCycleOffsetY ();
			oldJ = j;
		}
		if(totalCycleOffset <= 0f)
		{
			totalCycleOffset = totalCycleOffset + 1f;
		}
		Debug.Log("i: " + i + ", j: " + j + ", offset: " + totalCycleOffset);
		return totalCycleOffset;
	}
	
	
	
	private void repartentProcessors()
	{
		foreach(ProcessorManager processor in processorGroup)
		{
			processor.transform.parent = this.transform;
		}
	}
}
