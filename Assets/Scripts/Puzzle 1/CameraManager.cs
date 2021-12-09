using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform[] targetRoom;
    public float transitionSpeed;

    Transform currentRoom;

    Vector3 puzzleCameraRotate = new Vector3(0,0,0);
    Vector3 roomCameraRotate = new Vector3(45,0,0);

    public GameObject pintu;
    //public GameObject pintu2;
    //public GameObject pintu3;

    // Start is called before the first frame update
    void Start()
    {
        CameraStartPos();
        RoomName();
    }

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentRoom = targetRoom[0];
            transform.eulerAngles = roomCameraRotate;
        }  

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentRoom = targetRoom[1];
            transform.eulerAngles = roomCameraRotate;
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentRoom = targetRoom[2];
            transform.eulerAngles = roomCameraRotate;
        } 

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentRoom = targetRoom[3];
            transform.eulerAngles = roomCameraRotate;
        } 

        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentRoom = targetRoom[4];
            transform.eulerAngles = puzzleCameraRotate;
        }      
    }

    
    void LateUpdate()
    {
        //Lerp position
        transform.position = Vector3.Lerp(transform.position, currentRoom.position, Time.deltaTime * transitionSpeed);
        
    }

    public void MoveCameraToRoom1()
    {
        Debug.Log("Test 2");

        currentRoom = targetRoom[1];
        transform.eulerAngles = roomCameraRotate;
        Debug.Log("Test 3");
    }

    public void MoveCameraToRoom2()
    {
        currentRoom = targetRoom[2];
        transform.eulerAngles = roomCameraRotate;
    }

    public void MoveCameraToRoom3()
    {
        currentRoom = targetRoom[3];
        transform.eulerAngles = roomCameraRotate;
    }

    public void MoveCameraToMainRoom()
    {
        currentRoom = targetRoom[0];
        transform.eulerAngles = roomCameraRotate;
    }

    public void CameraStartPos()
    {
        currentRoom = targetRoom[0];
        transform.eulerAngles = roomCameraRotate;
    }

    public void MoveCameraToPuzzle1()
    {
        currentRoom = targetRoom[4];
        transform.eulerAngles = roomCameraRotate;
    }

    public void RoomName()
    {
        pintu.name = "Room1";
    }
}
