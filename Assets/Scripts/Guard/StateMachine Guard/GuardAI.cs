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

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        ChangeState(new PatrolState(this));
    }

    private void Update()
    {
        if(currentState != null)
        {
            currentState.Execute();
        }
    }

    public void ChangeState(W_IState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        if(currentState != null )
        {
            currentState.Enter();
        }
    }

    public bool SeesPlayer()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, 10f))
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
        Debug.Log("ALERT ALERT!!!!");
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
}
