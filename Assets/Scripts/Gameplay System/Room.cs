using UnityEngine;

public class Room : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
            // Move camera to this room
            CameraController.Instance.MoveCameraToRoom(transform.position);
    }
}
