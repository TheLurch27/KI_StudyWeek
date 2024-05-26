using UnityEngine;
using UnityEngine.UI;

public class EndZoneController : MonoBehaviour
{
    [SerializeField]
    private GameObject OpenDoor;
    [SerializeField]
    private GameObject ClosedDoor;
    [SerializeField]
    private float detectionRadius = 2.0f;
    private Transform PlayerTransform;
    private bool isPlayerNear;

    [SerializeField]
    private GameObject KeyUIGreen;
    [SerializeField]
    private GameObject KeyUIRed;
    [SerializeField]
    private GameObject KeyUIBlue;
    [SerializeField]
    private GameObject KeyUIYellow;
    [SerializeField]
    private GameObject EndScreenCanvas;

    [SerializeField] private EndScreenManager endScreenManager;

    private void Start()
    {
        isPlayerNear = false;
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        OpenDoor.SetActive(false);
        if (EndScreenCanvas != null)
        {
            EndScreenCanvas.SetActive(false);
        }
    }

    private void Update()
    {
        if (Vector3.Distance(PlayerTransform.position, transform.position) < detectionRadius)
        {
            if (!isPlayerNear)
            {
                isPlayerNear = true;
                Debug.Log("Player detected within radius.");
            }
        }
        else
        {
            if (isPlayerNear)
            {
                isPlayerNear = false;
                Debug.Log("Player left the detection radius.");
            }
        }

        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && IsKeyAcquired())
        {
            ToggleDoorState();
        }
    }

    private bool IsKeyAcquired()
    {
        
        return KeyUIGreen != null && KeyUIGreen.activeSelf && KeyUIRed != null && KeyUIRed.activeSelf && KeyUIBlue != null && KeyUIBlue.activeSelf && KeyUIYellow != null && KeyUIYellow.activeSelf;
    }

    private void ToggleDoorState()
    {
        if (ClosedDoor.activeSelf)
        {
            ClosedDoor.SetActive(false);
            OpenDoor.SetActive(true);
            ShowEndScreen();
        }
        else
        {
            ClosedDoor.SetActive(true);
            OpenDoor.SetActive(false);
        }
    }

    private void ShowEndScreen()
    {
        
        endScreenManager.ShowRandomEndScreen();
    }

    public bool IsDoorOpen()
    {
        return OpenDoor.activeSelf;
    }

    public void CloseDoor()
    {
        ClosedDoor.SetActive(true);
        OpenDoor.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
