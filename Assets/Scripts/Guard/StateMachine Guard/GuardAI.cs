using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    public List<Transform> waypoints;
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private List<GuardAI> allGuards;
    public Transform player;

    [Header("Guard Speeds")]
    public float patrolSpeed = 2f;
    public float scoutSpeed = 4f;

    public GuardVision guardVision;
    private W_IState currentState;
    private bool inAlertState = false;
    private float alertTimer = 0f;
    private float alertDuration = 10f;
    private float calmDownDuration = 10f;
    private bool playerSeen = false;
    private Vector3 lastKnownPosition;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        guardVision = GetComponent<GuardVision>();
        if (guardVision == null)
        {
            Debug.LogError("GuardVision component not found.");
        }

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("No waypoints assigned to the guard.");
        }

        allGuards = new List<GuardAI>(FindObjectsOfType<GuardAI>());
        ChangeState(new PatrolState(this));
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }

        if (agent.velocity != Vector3.zero)
        {
            guardVision.UpdateVisionDirection(agent.velocity.normalized);
        }

        if (SeesPlayer())
        {
            PlayerSeen();
        }

        if (playerSeen)
        {
            alertTimer += Time.deltaTime;
            if (alertTimer >= alertDuration)
            {
                alertTimer = 0f;
                playerSeen = false;
                inAlertState = false;
                AlertOtherGuards(false);
            }
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
        return guardVision != null && guardVision.CanSeePlayer();
    }

    public void AlertOtherGuards(bool alert)
    {
        foreach (GuardAI guard in allGuards)
        {
            if (guard != this)
            {
                if (alert)
                {
                    guard.ChangeState(new ScoutState(guard));
                    guard.SetDestination(lastKnownPosition);  // Set the destination to the last known position of the player
                }
                else
                {
                    guard.ChangeState(new PatrolState(guard));
                }
            }
        }
    }

    public Transform GetNextWaypoint()
    {
        if (waypoints == null || waypoints.Count == 0)
            return null;

        Transform waypoint = waypoints[currentWaypointIndex];
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        return waypoint;
    }

    public void SetDestination(Vector3 destination)
    {
        if (agent != null)
        {
            agent.SetDestination(destination);
        }
    }

    public void SetPatrolSpeed()
    {
        if (agent != null)
        {
            agent.speed = patrolSpeed;
        }
    }

    public void SetScoutSpeed()
    {
        if (agent != null)
        {
            agent.speed = scoutSpeed;
        }
    }

    public Transform GetPlayerTransform()
    {
        return player;
    }

    public void PlayerSeen()
    {
        playerSeen = true;
        lastKnownPosition = player.position; // Update last known position
        if (!inAlertState)
        {
            inAlertState = true;
            AlertOtherGuards(true);
            ChangeState(new AlertState(this));
        }
    }

    public Vector3 GetLastKnownPosition()
    {
        return lastKnownPosition;
    }
}
