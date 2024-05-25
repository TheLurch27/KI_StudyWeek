using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CalmDownState : W_IState
{
    private GuardAI guard;
    private float calmDownTime = 10f;
    private float calmDownTimer = 0f;

    public CalmDownState(GuardAI guard)
    {
        this.guard = guard;
    }

    public void Enter()
    {
        Debug.Log("Entering Calm Down State");
        calmDownTimer = 0f;
    }

    public void Execute()
    {
        Debug.Log("Calm Down");
        calmDownTimer += Time.deltaTime;
        if (calmDownTimer >= calmDownTime)
        {
            guard.ChangeState(new PatrolState(guard));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Calm Down State");
    }
}
