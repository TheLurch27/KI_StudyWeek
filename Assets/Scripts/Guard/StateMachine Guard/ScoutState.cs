using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutState : W_IState
{
    private GuardAI guard;
    private float scoutDuration = 10f;
    private float scoutTimer = 0f;

    public ScoutState(GuardAI guard)
    {
        this.guard = guard;
    }

    public void Enter()
    {
        Debug.Log("Entering Scout State");
        guard.SetScoutSpeed();
        scoutTimer = 0f;
    }

    public void Execute()
    {
        Debug.Log("Scouting");

        if (guard.SeesPlayer())
        {
            scoutTimer = 0f;
            guard.SetDestination(guard.GetPlayerTransform().position);
            guard.PlayerSeen();
        }
        else
        {
            scoutTimer += Time.deltaTime;
            if (scoutTimer >= scoutDuration)
            {
                guard.ChangeState(new CalmDownState(guard));
            }
            else
            {
                // Move towards the last known position of the player
                guard.SetDestination(guard.GetLastKnownPosition());
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Scout State");
    }
}
