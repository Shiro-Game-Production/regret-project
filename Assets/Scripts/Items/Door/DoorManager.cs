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
        [SerializeField] private Transform doorOpenerTransform;
        [SerializeField] private bool isPlayerInside;
        [SerializeField] private bool isLocked;
        
        private Vector3 targetPosition, insidePosition, outsidePosition;
        private PlayerMovement playerMovement;
        
        private bool moveDoorOpener;
        private const float DOOR_MOVEMENT_SPEED = 1.2f;

        private void Awake()
        {
            playerMovement = PlayerMovement.Instance;
            insidePosition = insideTransform.position;
            outsidePosition = outsideTransform.position;
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

        private void Update()
        {
            // Move door opener
            if (moveDoorOpener)
                MoveDoorOpener(targetPosition);
        }

        /// <summary>
        /// Play open and close door animation
        /// </summary>
        /// <returns></returns>
        private IEnumerator DoorAnimation()
        {
            playerMovement.canMove = false;
            // Open the door
            targetPosition = insidePosition;
            moveDoorOpener = true;
            
            yield return new WaitUntil(() => !moveDoorOpener);
            
            // Move the player
            playerMovement.Move(isPlayerInside ? outsidePosition : insidePosition);
            isPlayerInside = !isPlayerInside;
            
            // Wait for 2 seconds and when player is not walking anymore
            yield return new WaitForSeconds(2f);
            yield return new WaitUntil(() => !PlayerMovement.Instance.IsWalking);

            // Close the door
            targetPosition = outsidePosition;
            moveDoorOpener = true;
            playerMovement.canMove = true;
        }
        
        /// <summary>
        /// Move the door opener to inside and outside
        /// </summary>
        /// <param name="targetPosition">Door opener target position</param>
        private void MoveDoorOpener(Vector3 targetPosition)
        {
            // Move door opener
            doorOpenerTransform.position = Vector3.MoveTowards(
                doorOpenerTransform.position, targetPosition, 
                DOOR_MOVEMENT_SPEED * Time.deltaTime);
            
            // Stop moving
            if (doorOpenerTransform.position == targetPosition)
                moveDoorOpener = false;
        }
    }
}