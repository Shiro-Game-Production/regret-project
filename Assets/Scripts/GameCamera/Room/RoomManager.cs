using System.Collections.Generic;
using Dialogue;
using UnityEngine;

namespace GameCamera.Room
{
    public class RoomManager : SingletonBaseClass<RoomManager>
    {
        [SerializeField] private List<RoomTrigger> roomTriggers;
        public bool detectRooms;

        private DialogueManager dialogueManager;
        private bool fixCamera;
        private int roomCount, roomIndex;

        private void Start()
        {
            dialogueManager = DialogueManager.Instance;
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
            if (roomCount == 1 && fixCamera &&
                dialogueManager.dialogueMode != DialogueMode.Pause)
            {
                // If dialogue is still playing, but have to fix camera, just update the position
                if(dialogueManager.DialogueIsPlaying){
                    roomTriggers[roomIndex].UpdateCameraPosition();
                } else { // When dialogue is finished, fix the camera position
                    detectRooms = false;
                    fixCamera = false;
                    roomTriggers[roomIndex].SetCameraPosition();
                }
            }
        }
    }
}