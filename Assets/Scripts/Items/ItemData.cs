using Actors;
using Dialogue;
using UnityEngine;

namespace Items
{
    public abstract class ItemData: MonoBehaviour
    {
        public enum ItemType { DialogueFirst, Normal }

        public ItemType itemType = ItemType.Normal;

        protected ActorManager actorManager;
        
        public abstract void HandleInteraction();

        protected virtual void HandleDialogue()
        {
            DialogueManager.Instance.SetDialogue(actorManager.currentDialogue);
        }
    }
}