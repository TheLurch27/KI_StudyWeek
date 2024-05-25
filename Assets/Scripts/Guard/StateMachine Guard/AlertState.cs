using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlertState : W_IState
{
    private GuardAI guard;

    public AlertState(GuardAI guard)
    {
        this.guard = guard;
    }

    public void Enter()
    {
        Debug.Log("Entering Alert State");
        guard.AlertOtherGuards();
    }

    public void Execute()
    {
        Debug.Log("Alerting others");
        // Immediately move to CalmDownState after alerting
        guard.ChangeState(new CalmDownState(guard));
    }

    public void Exit()
    {
        Debug.Log("Exiting Alert State");
    }
}
