using UnityEngine;
using System.Collections;

public class ProcessorGroupController : MonoBehaviour {
	public ProcessorManager[,] processorGroup;
	int patternVariant;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void initialize(ProcessorManager[,] processorGroup, int patternVariant)
	{
		this.processorGroup = processorGroup;
		repartentProcessors();
		processorGroupInitialSetup(patternVariant);
	}
	
	private void processorGroupInitialSetup(int patternVariant)
	{
		Debug.Log ("Initial Setup for pattern number " + patternVariant);
	}
	
	private void repartentProcessors()
	{
		foreach(ProcessorManager processor in processorGroup)
		{
			processor.transform.parent = this.transform;
		}
	}
}
