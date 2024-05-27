using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CalmDownState : W_IState
{
    private GuardAI guard;
    private float calmDownTime = 10f;
    private float calmDownTimer = 0f;
    private Vector3 searchAreaCenter;
    private float searchRadius = 20f;
    private Vector3 currentSearchPoint;
    private bool searchPointReached = true;

    public CalmDownState(GuardAI guard)
    {
        this.guard = guard;
    }

    public void Enter()
    {
        Debug.Log("Entering Calm Down State");
        calmDownTimer = 0f;
        searchAreaCenter = guard.GetLastKnownPosition(); // Search around the last known position
    }

    public void Execute()
    {
        Debug.Log("Calm Down");

        if (guard.SeesPlayer())
        {
            guard.ChangeState(new ScoutState(guard));
            return;
        }

        calmDownTimer += Time.deltaTime;
        if (calmDownTimer >= calmDownTime)
        {
            guard.ChangeState(new PatrolState(guard));
        }
        else
        {
            if (searchPointReached)
            {
                Vector3 randomDirection = Random.insideUnitCircle * searchRadius;
                randomDirection += searchAreaCenter;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, searchRadius, 1))
                {
                    currentSearchPoint = hit.position;
                    guard.SetDestination(currentSearchPoint);
                    searchPointReached = false;
                }
            }
            else
            {
                if (Vector3.Distance(guard.transform.position, currentSearchPoint) < 1f)
                {
                    searchPointReached = true;
                }
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Calm Down State");
    }
}
