using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutState : W_IState
{
    private GuardAI guard;
    private bool playerSeen = false;
    private float lostPlayerTime = 0f;
    private float timeToLosePlayer = 10f;

    public ScoutState(GuardAI guard)
    {
        this.guard = guard;
    }

    public void Enter()
    {
        Debug.Log("Entering Scout State");
    }

    public void Execute()
    {
        Debug.Log("Scouting");

        if (guard.SeesPlayer())
        {
            playerSeen = true;
            lostPlayerTime = Time.time + timeToLosePlayer; // Setze Zeitpunkt, zu dem der Spieler verloren geht
            guard.AlertOtherGuards(); // Alarmiere andere Wachen
        }
        else if (playerSeen && Time.time > lostPlayerTime)
        {
            guard.ChangeState(new CalmDownState(guard)); // Spieler für 10 Sekunden nicht mehr gesehen, wechsle zu CalmDown
        }
        else
        {
            guard.FollowPlayer(); // Folge dem Spieler
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Scout State");
    }
}
