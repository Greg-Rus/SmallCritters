using UnityEngine;
using System.Collections;
using System;

public class ProcessorPatternConfigurator  {

	private ProcessorFSM processorStateMachine;
	private ProcessorManager[,] processorGroup;
	private int oldI = 0;
	private int oldJ = 0;
	private float totalCycleOffset = 0;

	public ProcessorPatternConfigurator(ProcessorFSM processorStateMachine)
	{
		this.processorStateMachine = processorStateMachine;
	}


	public void DeployPatternToProcessorGroup(ProcessorManager[,] processorGroup, int pattern)
	{
		this.processorGroup = processorGroup;
	}
	
	private Func<int,int,float> SelectPatternFunction(int pattern)
	{
		switch (pattern)
		{
		case 6: return RightToLeft; break;
		default: return RightToLeft;
		}
	}
	
	private void SetupProcessorCycleCompletion(Func<int,int,float> columnOffset)
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
		return 1f / processorGroup.GetLength(0);
	}
	
	private float GetCycleOffsetY()
	{
		return 1f / processorGroup.GetLength(1);
	}
	
	private float RightToLeft(int i, int j)
	{
		if (i != oldI) 
		{
			totalCycleOffset += GetCycleOffsetX ();
			oldI = i;
		}
		return totalCycleOffset;
	}
	
	private float LeftToRight(int i, int j)
	{
		if(i == 0 && j == 0)
		{
			totalCycleOffset = 1f;
		}
		if (i != oldI) 
		{
			totalCycleOffset -= GetCycleOffsetX ();
			oldI = i;
		}
		return totalCycleOffset;
	}
	
	private float TopDown(int i, int j)
	{
		Debug.Log("i: " + i + ", j: " + j + ", offset: " + totalCycleOffset);
		if (j == 0)
		{
			totalCycleOffset = 0;
		} 
		else if (j != oldJ) 
		{
			totalCycleOffset += GetCycleOffsetY ();
			oldJ = j;
		}
		return totalCycleOffset;
	}
	
	private float BottomUp(int i, int j)
	{
		Debug.Log("i: " + i + ", j: " + j + ", offset: " + totalCycleOffset);
		if (j == 0)
		{
			totalCycleOffset = 1;
		} 
		else if (j != oldJ) 
		{
			totalCycleOffset -= GetCycleOffsetY ();
			oldJ = j;
		}
		return totalCycleOffset;
	}
	
	private float HorizontalStripes(int i, int j)
	{
		Debug.Log("i: " + i + ", j: " + j + ", offset: " + totalCycleOffset);
		if (j != oldJ) 
		{
			if(j % 2 == 0)
			{
				totalCycleOffset = 0f;
			}
			else totalCycleOffset = 0.5f;
		}
		return totalCycleOffset;
	}
	
	private float VerticalStripes(int i, int j)
	{
		Debug.Log("i: " + i + ", j: " + j + ", offset: " + totalCycleOffset);
		if (i != oldI) 
		{
			if(i % 2 == 0)
			{
				totalCycleOffset = 0f;
			}
			else totalCycleOffset = 0.5f;
		}
		return totalCycleOffset;
	}
	
	private float Checkered(int i, int j)
	{
		Debug.Log("i: " + i + ", j: " + j + ", offset: " + totalCycleOffset);
		if((i+j) % 2 == 0)
		{
			totalCycleOffset = 0f;
		}
		else totalCycleOffset = 0.5f;
		
		return totalCycleOffset;
	}
	
	private float BackSlashToLeft(int i, int j)
	{
		if(i != oldI)
		{
			totalCycleOffset = GetCycleOffsetX() * i;
			oldI = i;
		} 
		if (j != oldJ) 
		{
			totalCycleOffset += GetCycleOffsetY ();
			oldJ = j;
		}
		if(totalCycleOffset >= 1f)
		{
			totalCycleOffset = totalCycleOffset - 1f;
		}
		return totalCycleOffset;
	}
}
