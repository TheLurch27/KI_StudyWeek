using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GuardAI : MonoBehaviour
{
    private W_IState currentState;

    [Header("Patrol Settings")]
    public List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    private GameObject player;
    private float patrolSpeed = 2f;

    [Header("Waiting Time")]
    public float waitTimeAtWaypoint = 2f;
    private bool isWaiting = false;

    [Header("Raycast Settings")]
    public float raycastDistance = 10f;

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
    }

    /// <summary>
    /// Checks if the guard can see the player using a raycast.
    /// </summary>
    /// <returns>True if the guard sees the player, false otherwise.</returns>
    public bool SeesPlayer()
    {
        RaycastHit hit;
        Vector3 direction = GetMovementDirection();
        Debug.DrawRay(transform.position, direction * raycastDistance, Color.red);

        if (Physics.Raycast(transform.position, direction, out hit, raycastDistance))
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
        Debug.Log("ALERT ALERT!!!!");
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
}
