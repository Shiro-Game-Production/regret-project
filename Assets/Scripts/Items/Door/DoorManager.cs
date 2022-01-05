using System.Collections;
using Audios.SoundEffects;
using Player;
using UnityEngine;

namespace Items.Door
{
    public class DoorManager: ItemData
    {
        [Header("Door Parameters")]
        [SerializeField] private Transform openerInsideTransform;
        [SerializeField] private Transform openerOutsideTransform;
        [SerializeField] private Transform doorOpenerTransform;
        [SerializeField] private bool isLocked;
        
        [Header("Player Parameters")]
        [SerializeField] private Transform playerInsideTransform;
        [SerializeField] private Transform playerOutsideTransform;
        [SerializeField] private bool isPlayerInside;
        
        private Vector3 targetPosition;
        private PlayerMovement playerMovement;
        
        private bool moveDoorOpener;
        private bool isAnimating;
        private const float DOOR_MOVEMENT_SPEED = 1.2f;

        public bool IsAnimating => isAnimating;
        
        private void Awake()
        {
            playerMovement = PlayerMovement.Instance;
            
            doorOpenerTransform.gameObject.SetActive(!isLocked);
        }

        public override void HandleInteraction()
        {
            if (isLocked)
            {
                ItemNotificationManager.Instance.SetNotificationText("Pintu terkunci");
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
            isAnimating = true;
            playerMovement.canMove = false;
            // Open the door
            SoundEffectManager.Instance.Play(ListSoundEffect.DoorOpened);
            targetPosition = openerInsideTransform.position;
            moveDoorOpener = true;
            
            yield return new WaitUntil(() => !moveDoorOpener);
            
            // Move the player
            playerMovement.Movement.Move(isPlayerInside ? 
                playerOutsideTransform.position : playerInsideTransform.position);
            isPlayerInside = !isPlayerInside;
            
            // Wait for 2 seconds and when player is not walking anymore
            yield return new WaitForSeconds(2f);
            yield return new WaitUntil(() => !playerMovement.Movement.IsWalking);

            // Close the door
            targetPosition = openerOutsideTransform.position;
            moveDoorOpener = true;
            playerMovement.canMove = true;
            yield return new WaitForSeconds(0.5f);
            SoundEffectManager.Instance.Play(ListSoundEffect.DoorClosed);
            isAnimating = false;
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