using UnityEngine;

namespace Items
{
    public class DoorManager : ItemData
    {
        [SerializeField] private Transform insideTransform;
        [SerializeField] private Transform outsideTransform;
        [SerializeField] private bool isPlayerInside;
        [SerializeField] private bool isLocked;
        
        private Transform playerTransform;

        private void Awake()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        public override void HandleInteraction()
        {
            if (isLocked) return;
            
            // Black screen fade in
            
            // Move the player
            playerTransform.position = isPlayerInside 
                ? outsideTransform.position : insideTransform.position;
            isPlayerInside = !isPlayerInside;

            // Black screen fade out

        }
    }
}