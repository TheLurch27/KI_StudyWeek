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
        guard.AlertOtherGuards(true);
    }

    public void Execute()
    {
        Debug.Log("Alerting others");
        guard.ChangeState(new ScoutState(guard));
    }

    public void Exit()
    {
        Debug.Log("Exiting Alert State");
    }
}