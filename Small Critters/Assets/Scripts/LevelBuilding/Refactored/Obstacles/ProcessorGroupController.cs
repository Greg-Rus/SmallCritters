using UnityEngine;
using System.Collections;
using System;

public class ProcessorGroupController : MonoBehaviour {
	public ProcessorManager[,] processorGroup;
	int patternVariant;
	private IProcessorPatternConfiguration patternConfigurator;
	private IProcessorFSM processorGroupFSM;

	void Awake () 
	{
		if(patternConfigurator == null)
		{
			patternConfigurator = ServiceLocator.getService<IProcessorPatternConfiguration>();
		}
		if(processorGroupFSM == null)
		{
			processorGroupFSM = new ProcessorFSM();
		}
	}
	
	public void initialize(ProcessorManager[,] processorGroup)
	{
		this.processorGroup = processorGroup;
		RrepartentProcessors();
		ProcessorGroupInitialSetup();
	}
	

	void Update () 
	{
		foreach(ProcessorManager processor in processorGroup)
		{
			processorGroupFSM.UpdateHeatupPhase(processor);
		}
	}

	private void ProcessorGroupInitialSetup()
	{
		patternConfigurator.DeployPatternToProcessorGroup(processorGroup, processorGroupFSM);
	}
	
	private void RrepartentProcessors()
	{
		foreach(ProcessorManager processor in processorGroup)
		{
			processor.transform.parent = this.transform;
		}
	}
}
