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
    private GameObject KeyUI; // Referenz auf das UI-Element, das den Schlüssel anzeigt
    [SerializeField]
    private GameObject EndScreenCanvas; // Referenz auf das EndScreen-Canvas

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
        // Überprüfe, ob das Schlüssel-UI-Element aktiv ist
        return KeyUI != null && KeyUI.activeSelf;
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
        // Zeige ein zufälliges EndScreen an
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
