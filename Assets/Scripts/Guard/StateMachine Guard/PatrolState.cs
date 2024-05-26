using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : W_IState
{
    private GuardAI guard;
    private Transform currentWaypoint;

    public PatrolState(GuardAI guard)
    {
        this.guard = guard;
    }

    public void Enter()
    {
        Debug.Log("Entering Patrol State");
        guard.SetPatrolSpeed();
        currentWaypoint = guard.GetNextWaypoint();
        if (currentWaypoint != null)
        {
            guard.SetDestination(currentWaypoint.position);
        }
        else
        {
            Debug.LogError("No waypoints assigned.");
        }
    }

    public void Execute()
    {
        Debug.Log("Patrolling");

        if (guard.SeesPlayer())
        {
            guard.PlayerSeen();
            guard.ChangeState(new ScoutState(guard));
        }
        else
        {
            if (currentWaypoint != null && Vector3.Distance(guard.transform.position, currentWaypoint.position) < 1f)
            {
                currentWaypoint = guard.GetNextWaypoint();
                if (currentWaypoint != null)
                {
                    guard.SetDestination(currentWaypoint.position);
                }
                else
                {
                    Debug.LogError("No more waypoints to assign.");
                }
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }
}