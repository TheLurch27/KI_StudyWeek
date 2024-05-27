using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Der Spieler Transform
    public float smoothSpeed = 0.125f; // Geschwindigkeit des Bump-Effekts
    public Vector3 offset; // Offset der Kamera relativ zum Spieler

    public float leftBound; // Linke Grenze
    public float rightBound; // Rechte Grenze
    public float topBound; // Obere Grenze
    public float bottomBound; // Untere Grenze

    void FixedUpdate()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned.");
            return;
        }

        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Begrenzen der Kamera innerhalb der Szene
        float cameraHalfWidth = Camera.main.orthographicSize * ((float)Screen.width / Screen.height);
        float cameraHalfHeight = Camera.main.orthographicSize;

        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, leftBound + cameraHalfWidth, rightBound - cameraHalfWidth);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, bottomBound + cameraHalfHeight, topBound - cameraHalfHeight);

        transform.position = smoothedPosition;
    }

    void OnDrawGizmos()
    {
        // Zeichne die Begrenzungen im Editor
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(leftBound, topBound, 0), new Vector3(rightBound, topBound, 0));
        Gizmos.DrawLine(new Vector3(rightBound, topBound, 0), new Vector3(rightBound, bottomBound, 0));
        Gizmos.DrawLine(new Vector3(rightBound, bottomBound, 0), new Vector3(leftBound, bottomBound, 0));
        Gizmos.DrawLine(new Vector3(leftBound, bottomBound, 0), new Vector3(leftBound, topBound, 0));
    }
}
