using UnityEngine;
using System.Collections;
using System;

public class ProcessorPatternConfigurator :IProcessorPatternConfiguration  {

	private IProcessorFSM processorStateMachine;
	private ProcessorManager[,] processorGroup;
	private int oldI = -1;
	private int oldJ = -1;
	private float totalCycleOffset = 0;
	private float minimalOffset = 0; //TODO calculate minimal offset so that long processor sections are not impassible. Can be made smaller as difficulty rises, but within reason.
	private IProcessorGroupPatternSelection patternSelection;
	
	public ProcessorPatternConfigurator(IProcessorFSM processorStateMachine)
	{
		this.processorStateMachine = processorStateMachine;
		patternSelection = ServiceLocator.getService<IProcessorGroupPatternSelection>();
		minimalOffset = (1f / 7f) * 1f;
	}


	public void DeployPatternToProcessorGroup(ProcessorManager[,] processorGroup)
	{
		this.processorGroup = processorGroup;
		resetConfigurator();
		int pattern = patternSelection.GetNewProcessorGroupPattern();
		ConfigureProcessorsBasedOnPattern(SelectPatternFunction(pattern));
	}
	
	private void resetConfigurator()
	{
		oldI = -1;
		oldJ = -1;
		totalCycleOffset = 0;
	}
	
	private Func<int,int,float> SelectPatternFunction(int pattern)
	{
		switch (pattern)
		{
		case 1: return RightToLeft; break;
		case 2: return LeftToRight; break;
		case 3: return HorizontalStripes; break;
		case 4: return VerticalStripes; break;
		case 5: return Checkered; break;
		case 6: return TopDown; break;
		case 7: return BottomUp; break;
		case 8: return BackSlashToLeft; break;
		case 9: return BackSlashToRight; break;
		default: return RightToLeft;
		}
	}
	
	private void ConfigureProcessorsBasedOnPattern(Func<int,int,float> PatternOffset)
	{
		for(int i = 0 ; i < processorGroup.GetLength(0) ;++i)
		{
			for(int j = 0 ; j < processorGroup.GetLength(1) ;++j)
			{
				processorStateMachine.setCycleCompletion(processorGroup[i,j],PatternOffset(i,j));
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
	
	private float RightToLeft(int i, int j) //gives an interresting effect with minimalOffset mutlipier of 3 and 4.
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
	
	private float LeftToRight(int i, int j) //gives an interresting effect with minimalOffset mutlipier of 3 and 4.
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
		if(totalCycleOffset <= 0f)
		{
			totalCycleOffset = totalCycleOffset + 1f;
		}
		return totalCycleOffset;
	}
	
	private float TopDown(int i, int j)
	{
		if (j == 0)
		{
			totalCycleOffset = 0;
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
	
	private float BottomUp(int i, int j)
	{
		if (j == 0)
		{
			totalCycleOffset = 1;
		} 
		else if (j != oldJ) 
		{
			totalCycleOffset -= GetCycleOffsetY ();
			oldJ = j;
		}
		if(totalCycleOffset <= 0f)
		{
			totalCycleOffset = totalCycleOffset + 1f;
		}
		return totalCycleOffset;
	}
	
	private float HorizontalStripes(int i, int j)
	{
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
	
	private float BackSlashToRight(int i, int j)
	{
		if(i==0 & j == 0)
		{
			totalCycleOffset = 1f;
		}
		
		if(i != oldI)
		{
			totalCycleOffset = 1f - (GetCycleOffsetX() * i);		
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
		return totalCycleOffset;
	}
}
