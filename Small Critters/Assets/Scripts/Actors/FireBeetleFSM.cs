using UnityEngine;
using System.Collections;
using System;

public class FireBeetleFSM {
    FireBeetleController controller;
    public Action CurrentAction;

    public FireBeetleFSM(FireBeetleController controller)
    {
        this.controller = controller;
    }

    public void Reset()
    {
        CurrentAction = StayIdle;
        controller.data.state = FireBeetleState.Idle;
    }

    private void StayIdle()
    {
        //Dummy state;
    }

    public void StartFollowingPlayer()
    {
        controller.data.state = FireBeetleState.Following;
        CurrentAction = FollowPlayer;

    }

    private void FollowPlayer()
    {
        controller.UpdatePlayerLocation();
        controller.RotateToFacePlayer();
        if (controller.IsInRange(controller.data.attackDistanceMin))
        {
            StartEvadingPlayer();
        }
        else if (controller.IsInRange(controller.data.attackDistanceMax) &&
                 controller.IsRotationWithinError() &&
                 controller.IsReadyToFire())
        {
            StartAttackingPlayer();
        }
        controller.Move(controller.data.walkSpeed);

    }

    private void StartEvadingPlayer()
    {
        CurrentAction = EvadePlayer;
    }

    private void EvadePlayer()
    {
        controller.UpdatePlayerLocation();
        controller.RotateToFacePlayer();
        if (!controller.IsInRange(controller.data.attackDistanceMax))
        {
            StartFollowingPlayer();
        }
        else if (controller.IsInRange(controller.data.attackDistanceMax) &&
                 controller.IsRotationWithinError() &&
                 controller.IsReadyToFire())
        {
            StartAttackingPlayer();
        }
        controller.Move(-controller.data.walkSpeed);
    }

    private void StartAttackingPlayer()
    {
        controller.data.state = FireBeetleState.Attacking;
        CurrentAction = Attack;
        controller.Attack();
    }

    private void Attack()
    {
        controller.UpdatePlayerLocation();
        controller.RotateToFacePlayer();
    }
}
