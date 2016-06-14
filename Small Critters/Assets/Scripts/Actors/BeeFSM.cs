using UnityEngine;
using System.Collections;
using System;

public class BeeFSM : MonoBehaviour {
    BeeController controller;
    Action currentAction;
    float stateExitTime;
    //public float chargeTime;
    //public float flySpeed;
    //public float chargeSpeed;
    //public float chargeDistance;
    //public float stunTime;
    public float chaseTimeLeft;
    public BeeState state;
    // Use this for initialization
    void OnEnable()
    {
        currentAction = StayIdle;
        state = BeeState.Idle;
    }

    void Start ()
    {
        controller = GetComponent<BeeController>();
    }
	
	// Update is called once per frame
	void Update () {
        currentAction();
    }

    private void StayIdle()
    {
        //Dummy state.
    }

    private void StartBeingStunned()
    {
        stateExitTime = Time.timeSinceLevelLoad + controller.stunTime;
        state = BeeState.Stunned;
        controller.MakeBeeGrounded();
        controller.SetAnimation("Stunned");
        currentAction = StayStunned;
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
        state = BeeState.Following;
        currentAction = FollowPlayer;
        controller.MakeBeeAirborn();
        controller.SetAnimation("Fly");
    }

    private void FollowPlayer()
    {
        controller.UpdatePlayerLocation();
        controller.RotateToFacePlayer();
        controller.ApplyFlyingForce();
        if (controller.CheckIfInRange(controller.chargeDistance))
        {
            StartChargingAtPlayer();
        }
    }

    private void StartChargingAtPlayer()
    {
        controller.RotateToFacePlayer();
        state = BeeState.Charging;
        currentAction = Charge;
        controller.SetAnimation("Charge");
        stateExitTime = Time.timeSinceLevelLoad + controller.chargeTime;
        controller.RapidStop();
    }

    private void Charge()
    {
        controller.ApplyChargingForce();
        chaseTimeLeft = stateExitTime - Time.timeSinceLevelLoad;
        CheckStateExitConditions();
    }

    private void CheckStateExitConditions()
    {
        if (Time.timeSinceLevelLoad >= stateExitTime)
        {
            controller.UpdatePlayerLocation();
            if (controller.CheckIfInRange(controller.chargeDistance))
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
