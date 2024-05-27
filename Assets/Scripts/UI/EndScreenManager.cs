using UnityEngine;
using UnityEngine.SceneManagement; // Importiere die SceneManager-Klasse
using System.Collections;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject[] endScreens;
    [SerializeField] private AudioSource backgroundMusic; // Referenz zur AudioSource für die Hintergrundmusik

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
        StartCoroutine(FadeOutMusic()); // Startet das Ausblenden der Musik
    }

    // Methode zum Neuladen der aktuellen Szene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Methode zum Beenden des Spiels
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        // Stoppt das Spiel im Editor-Modus
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator FadeOutMusic()
    {
        float fadeDuration = 1.0f; // Dauer des Ausblendens in Sekunden
        float startVolume = backgroundMusic.volume;

        while (backgroundMusic.volume > 0)
        {
            backgroundMusic.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        backgroundMusic.Stop();
        backgroundMusic.volume = startVolume; // Zurücksetzen der Lautstärke für den nächsten Einsatz
    }
}
