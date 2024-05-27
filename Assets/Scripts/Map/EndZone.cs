using UnityEngine;
using UnityEngine.UI;

public class EndZoneController : MonoBehaviour
{
    [SerializeField] private GameObject OpenDoor;
    [SerializeField] private GameObject ClosedDoor;
    [SerializeField] private float detectionRadius = 2.0f;
    private Transform PlayerTransform;
    private bool isPlayerNear;

    [SerializeField] private GameObject KeyUIGreen;
    [SerializeField] private GameObject KeyUIRed;
    [SerializeField] private GameObject KeyUIBlue;
    [SerializeField] private GameObject KeyUIYellow;

    [SerializeField] private EndScreenManager endScreenManager;
    [SerializeField] private GameObject backgroundMusic;

    private void Start()
    {
        isPlayerNear = false;
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        OpenDoor.SetActive(false);
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
        return KeyUIGreen != null && KeyUIGreen.activeSelf &&
               KeyUIRed != null && KeyUIRed.activeSelf &&
               KeyUIBlue != null && KeyUIBlue.activeSelf &&
               KeyUIYellow != null && KeyUIYellow.activeSelf;
    }

    private void ToggleDoorState()
    {
        if (ClosedDoor.activeSelf)
        {
            ClosedDoor.SetActive(false);
            OpenDoor.SetActive(true);
            ShowEndScreen();
            DisableBackgroundMusic();
        }
        else
        {
            ClosedDoor.SetActive(true);
            OpenDoor.SetActive(false);
            EnableBackgroundMusic();
        }
    }

    private void ShowEndScreen()
    {
        endScreenManager.ShowRandomEndScreen();
    }

    private void DisableBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.SetActive(false);
        }
    }

    private void EnableBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.SetActive(true);
        }
    }

    public bool IsDoorOpen()
    {
        return OpenDoor.activeSelf;
    }

    public void CloseDoor()
    {
        ClosedDoor.SetActive(true);
        OpenDoor.SetActive(false);
        EnableBackgroundMusic();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
