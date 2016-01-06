using UnityEngine;
using System.Collections;
using System;

public class ProcessorGroupController : MonoBehaviour {
	public ProcessorManager[,] processorGroup;
	int patternVariant;
//	public IProcessorFSM processorStateMachine;
	private IProcessorPatternConfiguration patternConfigurator;
	private IProcessorFSM processorGroupFSM;

	// Use this for initialization
	void Awake () 
	{
//		if (processorStateMachine == null) 
//		{
//			processorStateMachine = ServiceLocator.getService<IProcessorFSM>();
//		}
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
		repartentProcessors();
		processorGroupInitialSetup();
	}
	
	// Update is called once per frame
	void Update () 
	{
		foreach(ProcessorManager processor in processorGroup)
		{
			processorGroupFSM.updateHeatupPhase(processor);
		}
	}
	

	private void processorGroupInitialSetup()
	{
		patternConfigurator.DeployPatternToProcessorGroup(processorGroup, processorGroupFSM);
	}
	
	private void repartentProcessors()
	{
		foreach(ProcessorManager processor in processorGroup)
		{
			processor.transform.parent = this.transform;
		}
	}
}
