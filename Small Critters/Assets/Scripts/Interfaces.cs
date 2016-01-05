using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ProcessorState {Cool, HeatingUp, Hot, CoolingDown};
public enum sectionBuilderType {clear, blade, processor};

public interface Imovement {
	
	void makeMove(Vector3 direction);
	void configure(GameController controller);
	void rotateToDirection(Vector3 direction);
}

public interface ISectionDesigning
{
	void buildNewRow(List<GameObject> row);
}

public interface ISectionBuilderSelection
{
	void selectNewSectionBuilder();
	void addSectionBuilder (ISectionBuilder sectionBuilder);
}

public interface ISectionBuilderConfiguration
{
	void configureSectionBuilder();
}

public interface ISectionBuilder
{
	void buildNewRow(List<GameObject> row);
	sectionBuilderType type {get;set;}
}
public interface IProcessorFSM
{
	void changeStateTimer(ProcessorState state, float time);
	void updateHeatupPhase(ProcessorManager processor);
	void setCycleCompletion(ProcessorManager processor, float cyclePercent);
}
public interface IProcessorPatternConfiguration
{
	void DeployPatternToProcessorGroup(ProcessorManager[,] processorGroup, int pattern);
}

public interface IBladeSectionLength
{
	int GetNewBladeSectionLenght();
}

public interface IProcessorSectionLenght
{
	int GetNewProcessorSectionLenght();
}
public interface IProcessorGroupPattern
{
	int GetNewProcessorGroupPattern();
}
