using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private GameObject OpenDoor;
    [SerializeField]
    private GameObject ClosedDoor;
    [SerializeField]
    private float detectionRadius = 2.0f;
    private Transform PlayerTransform;
    private bool isPlayerNear;

    private void Start()
    {
        isPlayerNear = false;
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        OpenDoor.SetActive(false);
    }

    private void Update()
    {
        if(Vector3.Distance(PlayerTransform.position, transform.position) < detectionRadius)
        {
            isPlayerNear = true;
        }
        else
        {
            isPlayerNear = false;
        }
        if(isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoorState();
        }
    }

    private void ToggleDoorState()
    {
        if(ClosedDoor.activeSelf)
        {
            ClosedDoor.SetActive(false);
            OpenDoor.SetActive(true);
        }
        else
        {
            ClosedDoor.SetActive(true);
            ClosedDoor.SetActive(false);
        }

        return;
    }
}