using GameCamera;
using UnityEngine;

public class CapsuleController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    
    private RoomCameraMovement roomCameraMovement;

    // Start is called before the first frame update
    private void Awake()
    {
        roomCameraMovement = RoomCameraMovement.Instance;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical);
        transform.Translate(movement * (movementSpeed * Time.deltaTime), Space.World);
    }

    private void OnTriggerEnter(Collider other) 
    {
        RoomManager roomManager = other.GetComponent<RoomManager>();
        if (roomManager != null)
        {
            if(roomManager.PlayerInRoom)
                roomCameraMovement.MoveCamera(roomManager.CameraTransform.position);
        }
    }
}
