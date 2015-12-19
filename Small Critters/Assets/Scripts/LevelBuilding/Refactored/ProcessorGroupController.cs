using UnityEngine;
using System.Collections;

public class ProcessorGroupController : MonoBehaviour {
	public ProcessorManager[,] processorGroup;
	int patternVariant;
	public IProcessorFSM processorStateMachine;

	// Use this for initialization
	void Start () {
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
	void Update () {
		foreach(ProcessorManager processor in processorGroup)
		{
			processorStateMachine.updateHeatupPhase(processor);
		}
	}
	

	
	private void processorGroupInitialSetup(int patternVariant)
	{
		Debug.Log ("Initial Setup for pattern number " + patternVariant);

		if (patternVariant == 1)
		{
			//float cycleOffset = processorStateMachin
			for(int i = 0 ; i < processorGroup.GetLength(0) ;++i)
			{
				for(int j = 0 ; i < processorGroup.GetLength(1) ;++i)
				{
					processorStateMachine.setCycleCompletion(processorGroup[i,j],0f);
				}
			}
		}
	}
	
	private void repartentProcessors()
	{
		foreach(ProcessorManager processor in processorGroup)
		{
			processor.transform.parent = this.transform;
		}
	}
}
