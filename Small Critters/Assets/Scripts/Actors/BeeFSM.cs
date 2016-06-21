using UnityEngine;
using System.Collections;
using System;

public class BeeFSM {
    BeeController controller;
    public Action CurrentAction;

    public BeeFSM(BeeController controller)
    {
        this.controller = controller;
        Reset();
    }

    public void Reset()
    {
        CurrentAction = StayIdle;
        controller.data.state = BeeState.Idle;
    }

    private void StayIdle()
    {
        //Dummy state.
    }

    private void StartBeingStunned()
    {
        controller.data.stateExitTime = Time.timeSinceLevelLoad + controller.data.stunTime;
        controller.data.state = BeeState.Stunned;
        controller.MakeBeeGrounded();
        controller.SetAnimation("Stunned");
        CurrentAction = StayStunned;
    }

    private void StayStunned()
    {
        CheckStateExitConditions();
    }

    public void PlayerDetected(GameObject player)
    {
        StartFollowingPalyer();
    }

    private void StartFollowingPalyer()
    {
        controller.data.state = BeeState.Following;
        CurrentAction = FollowPlayer;
        controller.MakeBeeAirborn();
        controller.SetAnimation("Fly");
    }

    private void FollowPlayer()
    {
        controller.UpdatePlayerLocation();
        controller.RotateToFacePlayer();
        controller.ApplyFlyingForce();
        if (controller.CheckIfInRange(controller.data.chargeDistance))
        {
            StartChargingAtPlayer();
        }
    }

    private void StartChargingAtPlayer()
    {
        controller.RotateToFacePlayer();
        controller.data.state = BeeState.Charging;
        CurrentAction = Charge;
        controller.SetAnimation("Charge");
        controller.data.stateExitTime = Time.timeSinceLevelLoad + controller.data.chargeTime;
        controller.RapidStop();
    }

    private void Charge()
    {
        controller.ApplyChargingForce();
        controller.data.chaseTimeLeft = controller.data.stateExitTime - Time.timeSinceLevelLoad;
        CheckStateExitConditions();
    }

    private void CheckStateExitConditions()
    {
        if (Time.timeSinceLevelLoad >= controller.data.stateExitTime)
        {
            controller.UpdatePlayerLocation();
            if (controller.CheckIfInRange(controller.data.chargeDistance))
            {
                StartChargingAtPlayer();
            }
            else StartFollowingPalyer();
        }
    }

    public void OnColision()
    {
        StartBeingStunned();
    }

    public void OnPlayerDetected()
    {
        StartFollowingPalyer();
    }
}
