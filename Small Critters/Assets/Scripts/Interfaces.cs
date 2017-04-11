using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum ProcessorState {Cool, HeatingUp, Hot, CoolingDown};
public enum SectionBuilderType {clear, blade, processor, heatVent, bees, bugs};
public enum HeatVentState {Start, Closed, Opening, WarmingUp, Venting, Closing};
public enum BugType {None, Fly, FireBeetle, Bee };
public enum FireBeetleState { Idle, Following, Attacking };
public enum Trend { Falling = -1, Rising = 1};
public enum SwipeDirection {Backward = -1, Forward = 1};
public enum HorizontalDirection { Left = -1, Right = 1};
public enum BeeState { Idle, Following, Charging, Stunned };
public enum Toggled { On, Off};
public enum Sound
{
    Silence, Jump, ShotgunBlastAndCock, ShotgunFire, ShotgunCock, BeeCharge, BeeStunHit, FullHeart, StarPickup, BeatleCharge, BeatleSpit,
    EatFly, StartPowerup, KilledByFire, KilledByImpact, PlayerHit, PlayerKilled, Notification, SummaryText, SummaryMutliplyer, SummaryScore
};
public enum NotificationType {
    Undefined, BladeKill, ProcessorKill, VentKill, ColdKill, OtherBee, OtherBeetle,
    SingleKill, DoubleKill, TrippleKill, QuadKill, QuintKill, Overkill, RiskyJump
};
public enum ScoreStarState { Starting, Waiting, Decaying, Following, BeingPickedUp};

public interface Imovement {
	
	void makeMove(Vector3 direction);
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
	SectionBuilderType type {get;set;}
}
public interface IProcessorFSM
{
	void UpdateHeatupPhase(ProcessorManager processor);
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
    float GetStunTime();
	int GetNewBeeSectionLength();
	bool IsBeePresent();
}

public interface IFireBeetleDifficultyManager
{
    float GetWalkSpeed();
    float GetRotationSpeed();
    float GetAttackDistanceMax();
    float GetAttackDistanceMin();
    float GetShotCooldownTime();
}

public interface IBugsSectionDifficulty
{
    int GetNewBugSectionLength();
    BugType GetBugType();
    float difficultyPercent { get; }
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
	SectionBuilderType GetSectionBuilder();
    void BanSectionType(SectionBuilderType sectionTypeToBan);
}

public interface IArenaBuilding
{
	void SetUpArenaRow(List<GameObject> row);
}

public interface IPlayerDetection
{
    void PlayerDetected(GameObject player);
}

public interface IResetable
{
    void Reset(Action<GameObject> storeInPool);
}

public interface IDeathReporting
{
    void EnemyDead(GameObject enemy, string causeOfDeath);
}

public interface IGameProgressReporting
{
    void NewRowsReached(int rows);
    void RunEnd(string cuseOfDeath);
}

public interface IScoreForUI
{
    ScoreData GetScoreData();
    void RestartRun(int button);
    void RestartGame();
    void SpeedUpSummary();
}

public interface IPowerup
{
    void UpdatePoints(float points);
    void OnShotFired();
    void SetBonus();
    bool powerupModeOn { get; }
}

public interface IAudio
{
    void PlaySound(Sound sound);
    void PlayEnemyDeathSound(string causeOfDeath);
    void PauseAudio();
    void UnPauseAudio();
    void SetMusicOn(bool state);
    void SetSoundFXOn(bool state);
    void PlayPowerupMusic(bool powerUpOn);

}





