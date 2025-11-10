using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    
    [Header("Components")]
    [SerializeField] private GameObject virutalCamera;
    [SerializeField] private Transform startingRoom;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        virutalCamera.transform.position = new Vector3
        (
            startingRoom.position.x,
            startingRoom.position.y,
            virutalCamera.transform.position.z
        );
    }
    
    // TODO: Use a corroutine for smooth cam movement
    public void MoveCameraToRoom(Vector3 roomPosition)
    {
        virutalCamera.transform.position = new Vector3
        (
            roomPosition.x,
            roomPosition.y,
            virutalCamera.transform.position.z
        );
    }
}
