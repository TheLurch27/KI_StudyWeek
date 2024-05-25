using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardVision : MonoBehaviour
{
    public float viewAngle = 90f;
    public float viewDistance = 10f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    private Transform player;
    private bool canSeePlayer;

    private Transform fovVisual;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        fovVisual = transform.Find("FOVVisual");
        if (fovVisual == null)
        {
            Debug.LogError("FOVVisual object not found. Please create a child object named 'FOVVisual' for the field of view visualization.");
        }
    }

    void Update()
    {
        canSeePlayer = CheckIfPlayerInSight();
        UpdateFOVVisual();
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
                if (!Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void UpdateFOVVisual()
    {
        if (fovVisual == null)
            return;

        // Set FOVVisual position and rotation to match the guard
        fovVisual.position = transform.position;
        fovVisual.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z - viewAngle / 2f);
    }

    void OnDrawGizmos()
    {
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
}