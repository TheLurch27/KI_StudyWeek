using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : W_IState
{
    private GuardAI guard;
    public PatrolState(GuardAI guard)
    {
        this.guard = guard;
    }

    public void Enter()
    {
        Debug.Log("Entering Patrol State");
    }

    public void Execute()
    {
        Debug.Log("Patrolling");

        if (guard.SeesPlayer())
        {
            guard.ChangeState(new ScoutState(guard));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }
}