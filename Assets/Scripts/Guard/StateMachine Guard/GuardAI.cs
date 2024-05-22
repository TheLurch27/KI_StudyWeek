using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAI : MonoBehaviour
{
    private W_IState currentState;

    [Header("Patrol Settings")]
    public List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    public GameObject player;
    public float patrolSpeed = 2f;

    [Header("Waiting Time")]
    public float waitTimeAtWaypoint = 2f;
    private bool isWaiting = false;
    private bool hasAlertedGuards = false;

    [Header("Raycast Settings")]
    public float raycastDistance = 10f;
    private Vector3 raycastDirection = Vector3.right;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        ChangeState(new PatrolState(this));
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }

    /// <summary>
    /// Changes the current state of the guard to the new state.
    /// </summary>
    /// <param name="newState">The new state to transition to.</param>
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

        hasAlertedGuards = false;
    }

    /// <summary>
    /// Checks if the guard can see the player using a raycast.
    /// </summary>
    /// <returns>True if the guard sees the player, false otherwise.</returns>
    public bool SeesPlayer()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);

        if (Physics.Raycast(transform.position, raycastDirection, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Determines the direction the guard is moving towards based on waypoints.
    /// </summary>
    /// <returns>The normalized direction vector towards the next waypoint.</returns>
    private Vector3 GetMovementDirection()
    {
        Vector3 direction = Vector3.zero;

        if (currentWaypointIndex < waypoints.Count)
        {
            Vector3 nextWaypoint = waypoints[currentWaypointIndex].position;
            Vector3 currentPosition = transform.position;
            direction = (nextWaypoint - currentPosition).normalized;
        }

        return direction;
    }

    /// <summary>
    /// Alerts other guards that the player has been spotted.
    /// </summary>
    public void AlertOtherGuards()
    {
        if (!hasAlertedGuards) // Check if guards have been alerted
        {
            Debug.Log("ALERT ALERT!!!!");
            hasAlertedGuards = true; // Set the flag to true to indicate that guards have been alerted
        }
    }

    /// <summary>
    /// Patrols between waypoints. Moves the guard towards the current waypoint.
    /// </summary>
    public void Patrol()
    {
        if (waypoints.Count == 0 || isWaiting) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, patrolSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    /// <summary>
    /// Waits at the current waypoint for the specified time before moving to the next waypoint.
    /// </summary>
    private IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimeAtWaypoint);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        isWaiting = false;
    }

    public void FollowPlayer()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        // Bewegung des Wächters in Richtung des Spielers
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, patrolSpeed * Time.deltaTime);
        // Ausrichtung des Wächters in Richtung des Spielers
        transform.right = directionToPlayer;
        // Aktualisierung des Raycasts basierend auf der neuen Ausrichtung
        raycastDirection = directionToPlayer;
    }
}
