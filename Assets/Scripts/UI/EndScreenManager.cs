using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject[] endScreens;

    private void Start()
    {
        foreach (GameObject endScreen in endScreens)
        {
            endScreen.SetActive(false);
        }
    }

    public void ShowRandomEndScreen()
    {
        int randomIndex = Random.Range(0, endScreens.Length);
        GameObject randomEndScreen = endScreens[randomIndex];

        randomEndScreen.SetActive(true);
    }
}