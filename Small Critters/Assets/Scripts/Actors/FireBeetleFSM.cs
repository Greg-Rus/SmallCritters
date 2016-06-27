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
        controller.SetSpeed(controller.data.walkSpeed);
    }

    private void FollowPlayer()
    {
        controller.UpdatePlayerLocation();
        controller.RotateToFacePlayer();
        controller.UpdateAnimationSpeed();
        FollowPlayerExitCheck();
    }

    private void FollowPlayerExitCheck()
    {
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
    }

    private void StartEvadingPlayer()
    {
        CurrentAction = EvadePlayer;
        controller.SetSpeed(-controller.data.walkSpeed);
    }

    private void EvadePlayer()
    {
        controller.UpdatePlayerLocation();
        controller.RotateToFacePlayer();
        controller.UpdateAnimationSpeed();
        EvadePlayerExitCheck();
    }

    private void EvadePlayerExitCheck()
    {
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
