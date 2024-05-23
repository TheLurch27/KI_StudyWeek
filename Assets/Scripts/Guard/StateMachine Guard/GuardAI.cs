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

    [Header("Raycast Settings")]
    public float raycastDistance = 10f;
    private Vector3 raycastDirection = Vector3.right;

    private bool hasAlertedGuards = false;

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

    public void AlertOtherGuards()
    {

        if (!hasAlertedGuards)
        {
            Debug.Log("ALERT ALERT!!!!");
            hasAlertedGuards = true;
        }

    }

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
        
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, patrolSpeed * Time.deltaTime);
        
        transform.right = directionToPlayer;
        
        raycastDirection = directionToPlayer;
    }
}