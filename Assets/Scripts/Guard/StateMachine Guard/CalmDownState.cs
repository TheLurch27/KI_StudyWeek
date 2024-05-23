using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalmDownState : W_IState
{
    private GuardAI guard;
    private float calmDownTime = 5f;

    public CalmDownState(GuardAI guard)
    {
        this.guard = guard;
    }

    public void Enter()
    {
        Debug.Log("Entering Calm Down State");
        guard.StartCoroutine(CalmDownRoutine());
    }

    public void Execute()
    {
        Debug.Log("Calm Down");
    }

    public void Exit()
    {
        Debug.Log("Exiting Calm Down State");
    }

    private IEnumerator CalmDownRoutine()
    {
        yield return new WaitForSeconds(calmDownTime);
        guard.ChangeState(new PatrolState(guard));
    }
}