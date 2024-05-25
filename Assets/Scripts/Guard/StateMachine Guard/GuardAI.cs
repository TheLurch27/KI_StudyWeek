using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    public GameObject player;
    private W_IState currentState;
    public List<Transform> waypoints;
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private List<GuardAI> allGuards;

    [Header("Guard Speeds")]
    public float patrolSpeed = 2f;
    public float scoutSpeed = 4f;

    private GuardVision guardVision;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        allGuards = new List<GuardAI>(FindObjectsOfType<GuardAI>());
        guardVision = GetComponent<GuardVision>();
        ChangeState(new PatrolState(this));
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }

    public void ChangeState(W_IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.Enter();
        }
    }

    public bool SeesPlayer()
    {
        return guardVision.CanSeePlayer();
    }

    public void AlertOtherGuards()
    {
        foreach (GuardAI guard in allGuards)
        {
            if (guard != this)
            {
                guard.ChangeState(new AlertState(guard));
            }
        }
    }

    public Transform GetNextWaypoint()
    {
        if (waypoints.Count == 0)
            return null;

        Transform waypoint = waypoints[currentWaypointIndex];
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        return waypoint;
    }

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void SetPatrolSpeed()
    {
        agent.speed = patrolSpeed;
    }

    public void SetScoutSpeed()
    {
        agent.speed = scoutSpeed;
    }
}
