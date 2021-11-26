using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform[] targetRoom;
    public float transitionSpeed;

    Transform currentRoom;

    //public GameObject pintu;
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
        }  

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentRoom = targetRoom[1];
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentRoom = targetRoom[2];
        } 

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentRoom = targetRoom[3];
        } 

        //MoveCamera();      
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
        //currentRoom = targetRoom[1];
        // if(pintu.name == "Room1")
        // {
        //     currentRoom = targetRoom[1];
        // }

        // if(pintu2.name == "Room2")
        // {
        //     currentRoom = targetRoom[2];
        // }

        // if(pintu3.name == "Room3")
        // {
        //     currentRoom = targetRoom[3];
        // }
        Debug.Log("Test 3");
    }

    public void MoveCameraToRoom2()
    {
        currentRoom = targetRoom[2];
    }

    public void MoveCameraToRoom3()
    {
        currentRoom = targetRoom[3];
    }

    public void MoveCameraToMainRoom()
    {
        currentRoom = targetRoom[0];
    }

    public void CameraStartPos()
    {
        currentRoom = targetRoom[0];
    }

    public void RoomName()
    {
        //pintu.name = "Room1";
        // pintu2.name = "Room2";
        // pintu3.name = "Room3";
    }
}
