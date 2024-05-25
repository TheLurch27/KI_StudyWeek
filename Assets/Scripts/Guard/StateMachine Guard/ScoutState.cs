using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoutState : W_IState
{
    private GuardAI guard;

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

        if (!guard.SeesPlayer())
        {
            guard.ChangeState(new CalmDownState(guard));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Scout State");
    }
}