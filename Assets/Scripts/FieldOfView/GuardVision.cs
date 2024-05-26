using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardVision : MonoBehaviour
{
    public float viewAngle = 90f;
    public float viewDistance = 10f;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    [Header("Gizmos Settings")]
    public bool showViewGizmos = true;

    private Transform player;
    private bool canSeePlayer;
    private GuardAI guardAI;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        guardAI = GetComponent<GuardAI>();
    }

    void Update()
    {
        bool previousCanSeePlayer = canSeePlayer;
        canSeePlayer = CheckIfPlayerInSight();

        if (canSeePlayer && !previousCanSeePlayer)
        {
            guardAI.ChangeState(new ScoutState(guardAI));
        }
        else if (!canSeePlayer && previousCanSeePlayer)
        {
            guardAI.ChangeState(new CalmDownState(guardAI));
        }
    }

    private bool CheckIfPlayerInSight()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.right, directionToPlayer);

        if (angleToPlayer < viewAngle / 2f)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer < viewDistance)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, viewDistance, obstructionMask | targetMask);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.Log("Player detected");
                        return true;
                    }
                    else
                    {
                        Debug.Log("Obstruction detected: " + hit.collider.name);
                    }
                }
                else
                {
                    Debug.Log("No obstruction, player in sight");
                }
            }
        }

        return false;
    }

    void OnDrawGizmos()
    {
        if (!showViewGizmos) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewDistance);

        if (canSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }

    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    public bool CanSeePlayer()
    {
        return canSeePlayer;
    }

    public void UpdateVisionDirection(Vector3 direction)
    {
        transform.right = direction;
    }
}
