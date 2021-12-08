using System.Collections.Generic;
using Dialogue;
using UnityEngine;

namespace GameCamera.Room
{
    public class RoomManager : SingletonBaseClass<RoomManager>
    {
        [SerializeField] private List<RoomTrigger> roomTriggers;
        public bool detectRooms;
        private bool fixCamera;
        private int roomCount, roomIndex;

        private void Start()
        {
            fixCamera = true;
        }

        private void Update()
        {
            // Don't detect room if detect rooms is false
            if (!detectRooms) return;
            
            // Detect room
            roomCount = 0;
            for (var i = 0; i < roomTriggers.Count; i++)
            {
                RoomTrigger roomTrigger = roomTriggers[i];
                if (roomTrigger.playerInRoom)
                {
                    roomCount++;
                    roomIndex = i;
                }
            }
            
            // If player is in 2 rooms, fix camera is true
            if (roomCount > 1)
            {
                fixCamera = true;
            }
            
            // When player is only in 1 room, set camera position
            if (roomCount == 1 && fixCamera && !DialogueManager.Instance.DialogueIsPlaying)
            {
                detectRooms = false;
                fixCamera = false;
                roomTriggers[roomIndex].SetCameraPosition();
            }
        }
    }
}