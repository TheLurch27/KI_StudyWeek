using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutState : W_IState
{
    private GuardAI guard;
    private bool playerSeen = false;
    private float lostPlayerTime = 0f;
    private float timeToLosePlayer = 10f;

    public ScoutState(GuardAI guard)
    {
        this.guard = guard;
    }

    public void Enter()
    {
        Debug.Log("Entering Scout State");
    }

    public void Execute()
    {
        Debug.Log("Scouting");

        if (guard.SeesPlayer())
        {
            playerSeen = true;
            lostPlayerTime = Time.time + timeToLosePlayer;
            guard.AlertOtherGuards();
        }
        else if (playerSeen && Time.time > lostPlayerTime)
        {
            guard.ChangeState(new CalmDownState(guard));
        }
        else
        {
            guard.FollowPlayer();
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Scout State");
    }
}