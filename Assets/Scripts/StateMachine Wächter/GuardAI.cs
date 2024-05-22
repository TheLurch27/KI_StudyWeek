using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GuardAI : MonoBehaviour
{
    private GameObject player;
    private W_IState currentState;

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
}
