using UnityEngine;
using System.Collections;
using System;

public class ProcessorGroupController : MonoBehaviour {
	public ProcessorManager[,] processorGroup;
	int patternVariant;
	public IProcessorFSM processorStateMachine;
	private IProcessorPatternConfiguration patternConfigurator;

	//TODO separate class
	int oldI = -1;
	int oldJ = -1;
	float totalCycleOffset = 0;
	private float minimalOffset = 0.2857f;

	// Use this for initialization
	void Awake () 
	{
		if (processorStateMachine == null) 
		{
			processorStateMachine = ServiceLocator.getService<IProcessorFSM>();
		}
		if(patternConfigurator == null)
		{
			patternConfigurator = ServiceLocator.getService<IProcessorPatternConfiguration>();
		}
		minimalOffset = (1f / 7f) * 4f;
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
		int pattern = UnityEngine.Random.Range(1,9);
		patternConfigurator.DeployPatternToProcessorGroup(processorGroup, pattern);
		//Debug.Log ("Initial Setup for pattern number " + patternVariant);
		/*
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
		*/
	}
/*
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

	private float GetCycleOffsetX()
	{
		float offset = 1f / processorGroup.GetLength(0);
		if(offset < minimalOffset)
		{
			offset = minimalOffset; 
		}
		return offset;
	}
	
	private float GetCycleOffsetY()
	{
		float offset = 1f / processorGroup.GetLength(1);
		if(offset < minimalOffset)
		{
			offset = minimalOffset; 
		}
		return offset;
	}

	private float TopDown(int i, int j)
	{
		if (i != oldI) 
		{
			totalCycleOffset += GetCycleOffsetX ();
			oldI = i;
		}

		if(totalCycleOffset >= 1f)
		{
			totalCycleOffset = totalCycleOffset - 1f;
		}
		return totalCycleOffset;
	}
*/	
	
	
	private void repartentProcessors()
	{
		foreach(ProcessorManager processor in processorGroup)
		{
			processor.transform.parent = this.transform;
		}
	}
}
