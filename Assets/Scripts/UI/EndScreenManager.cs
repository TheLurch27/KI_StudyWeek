using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject[] endScreens;

    private void Start()
    {
        // Deaktiviere alle EndScreens zu Beginn
        foreach (GameObject endScreen in endScreens)
        {
            endScreen.SetActive(false);
        }
    }

    public void ShowRandomEndScreen()
    {
        // Wähle zufällig ein EndScreen aus dem Array aus
        int randomIndex = Random.Range(0, endScreens.Length);
        GameObject randomEndScreen = endScreens[randomIndex];

        // Aktiviere das ausgewählte EndScreen
        randomEndScreen.SetActive(true);
    }
}