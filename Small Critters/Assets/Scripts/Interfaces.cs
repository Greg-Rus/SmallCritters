using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ProcessorState {Cool, HeatingUp, Hot, CoolingDown};
public enum sectionBuilderType {clear, blade, processor, heatVent, bees};
public enum HeatVentState {Start, Closed, Opening, WarmingUp, Venting, Closing};

public interface Imovement {
	
	void makeMove(Vector3 direction);
	void configure(GameController controller);
	void rotateToDirection(Vector3 direction);
	bool midJump{get;set;}
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
	void updateHeatupPhase(ProcessorManager processor);
	void SetStateTimes(float[] timers);
	void SetCycleCompletion(ProcessorManager processor, float cyclePercent);
}
public interface IProcessorPatternConfiguration
{
	void DeployPatternToProcessorGroup(ProcessorManager[,] processorGroup, IProcessorFSM processorGroupFSM);
}

public interface IBladeSectionDifficulty
{
	int GetNewBladeSectionLenght();
	float GetBladeGap();
	float GetBladeSpeed();
	bool IsBladeRowEmpty();
	float GetBladeRowCycleOffset();
	
}

public interface IHeatVentSectionDifficulty
{
	bool IsHeatVentRowEmpty();
	float[] GetHeatVentFSMTimers();
	float GetHeatVentLength();
	Vector3 GetHeatVentRotation();
	float GetHeatVentCycleOffset();
	int GetNewHeatVentSectionLenght();
}

public interface IProcessorGroupDifficulty
{
	int GetNewProcessorSectionLenght();
	int GetNewProcessorGroupPattern();
	float GetProcessorPatternCycleOffset();
	float[] GetProcessorFSMTimers();
}

public interface IBeeSectionDifficulty
{
	float GetChargeTime();
	float GetFlySpeed();
	float GetChargeSpeed();
	float GetChargeDistance();
	int GetNewBeeSectionLength();
	bool IsBeePresent();
}

public interface IRowCleanup
{
	void DismantleRow(List<GameObject> row);
}

public interface IDifficultyScaling
{
	void ScaleDifficulty();
}

public interface IDifficultyBasedBuilderPicking
{
	sectionBuilderType GetSectionBuilder();
}

public interface IArenaBuilding
{
	void SetUpArenaRow(List<GameObject> row);
}



