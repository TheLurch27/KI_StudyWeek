using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField]
    private GameObject endScreenCanvas; // Referenz auf das EndScreen-Canvas
    private VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component not found on the GameObject.");
            return;
        }

        videoPlayer.playOnAwake = false; // Video soll nicht automatisch beim Start abgespielt werden

        if (endScreenCanvas != null)
        {
            endScreenCanvas.SetActive(false); // Stelle sicher, dass das EndScreen-Canvas zunächst deaktiviert ist
        }
    }

    private void Update()
    {
        if (endScreenCanvas != null && endScreenCanvas.activeSelf && !videoPlayer.isPlaying)
        {
            PlayVideo();
        }
    }

    public void PlayVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play();
            Debug.Log("End screen video started.");
        }
    }
}
