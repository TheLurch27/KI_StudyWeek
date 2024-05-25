using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeController : MonoBehaviour
{
    [SerializeField]
    private GameObject SafeOpen;
    [SerializeField]
    private GameObject SafeClosed;
    [SerializeField]
    private GameObject KeyCard;
    [SerializeField]
    private float detectionRadius = 2.0f;
    private Transform PlayerTransform;
    private bool isPlayerNear;

    private void Start()
    {
        isPlayerNear = false;
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SafeOpen.SetActive(false);
    }

    private void Update()
    {
        if (Vector3.Distance(PlayerTransform.position, transform.position) < detectionRadius)
        {
            isPlayerNear = true;
        }
        else
        {
            isPlayerNear = false;
        }
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ToggleSafeState();
        }
    }

    private void ToggleSafeState()
    {
        if (SafeClosed.activeSelf)
        {
            SafeClosed.SetActive(false);
            SafeOpen.SetActive(true);
            KeyCard.SetActive(true);
        }
        else
        {
            SafeOpen.SetActive(true);
            SafeClosed.SetActive(false);
        }
    }
}
