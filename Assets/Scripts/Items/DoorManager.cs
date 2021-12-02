using System;
using System.Collections;
using Actors;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Items
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshObstacle))]
    public class DoorManager: ItemData
    {
        [Header("Door Parameters")]
        [SerializeField] private Transform insideTransform;
        [SerializeField] private Transform outsideTransform;
        [SerializeField] private bool isPlayerInside;
        [SerializeField] private bool isLocked;

        private Animator animator;
        private NavMeshObstacle navMeshObstacle;
        private PlayerMovement playerMovement;
        private static readonly int OpenDoor = Animator.StringToHash("openDoor");
        private static readonly int CloseDoor = Animator.StringToHash("closeDoor");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            navMeshObstacle = GetComponent<NavMeshObstacle>();
            playerMovement = PlayerMovement.Instance;

            navMeshObstacle.carving = true;
        }

        public override void HandleInteraction()
        {
            if (isLocked) return;
            // Play door animation
            StartCoroutine(DoorAnimation());
            
            switch (itemMode)
            {
                case ItemMode.DialogueMode:
                    actorManager = GetComponent<ActorManager>();
                    HandleDialogue();
                    break;
                case ItemMode.NormalMode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Play open and close door animation
        /// </summary>
        /// <returns></returns>
        private IEnumerator DoorAnimation()
        {
            playerMovement.canMove = false;
            // Open the door
            animator.SetTrigger(OpenDoor);
            
            // Move the player
            playerMovement.Move(isPlayerInside 
                ? outsideTransform.position : insideTransform.position);
            isPlayerInside = !isPlayerInside;
            
            yield return new WaitForSeconds(2f);

            // Close the door
            animator.SetTrigger(CloseDoor);
            playerMovement.canMove = true;
        }
    }
}