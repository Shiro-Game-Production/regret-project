using UnityEngine;

public class CameraManager : SingletonBaseClass<CameraManager>
{
    [SerializeField] private Transform[] targetRoom;
    [SerializeField] private float transitionSpeed;

    private Transform currentRoom;

    // Start is called before the first frame update
    private void Start()
    {
        MoveCamera(0);
    }
    
    private void LateUpdate()
    {
        // Lerp position
        transform.position = Vector3.Lerp(transform.position, currentRoom.position, Time.deltaTime * transitionSpeed);
    }

    public void MoveCamera(int index)
    {
        currentRoom = targetRoom[index];
    }
}
