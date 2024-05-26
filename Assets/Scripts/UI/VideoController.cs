using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField]
    private GameObject endScreenCanvas;
    private VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.playOnAwake = false;

        if (endScreenCanvas != null)
        {
            endScreenCanvas.SetActive(false);
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
