using System.Collections;
using Player;
using UnityEngine;

namespace Items.Door
{
    public class DoorManager: ItemData
    {
        [Header("Door Parameters")]
        [SerializeField] private Transform insideTransform;
        [SerializeField] private Transform outsideTransform;
        [SerializeField] private bool isPlayerInside;
        [SerializeField] private bool isLocked;
        private PlayerMovement playerMovement;

        private void Awake()
        {
            playerMovement = PlayerMovement.Instance;
        }

        public override void HandleInteraction()
        {
            if (isLocked)
            {
                ItemNotificationManager.Instance.SetNotificationText("Door is locked");
                return;
            }
            // Play door animation
            StartCoroutine(DoorAnimation());
            base.HandleInteraction();
        }

        /// <summary>
        /// Play open and close door animation
        /// </summary>
        /// <returns></returns>
        private IEnumerator DoorAnimation()
        {
            playerMovement.canMove = false;
            // Open the door
            // animator.SetTrigger(OpenDoor);
            
            // Move the player
            playerMovement.Move(isPlayerInside 
                ? outsideTransform.position : insideTransform.position);
            isPlayerInside = !isPlayerInside;
            
            yield return new WaitForSeconds(2f);

            // Close the door
            // animator.SetTrigger(CloseDoor);
            playerMovement.canMove = true;
        }
    }
}