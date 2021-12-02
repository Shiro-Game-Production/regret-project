using Actors;
using Dialogue;
using UnityEngine;

namespace Items
{
    public abstract class ItemData: MonoBehaviour
    {
        public enum ItemMode { DialogueMode, NormalMode }

        public ItemMode itemMode = ItemMode.NormalMode;

        protected ActorManager actorManager;
        
        public abstract void HandleInteraction();

        protected virtual void HandleDialogue()
        {
            DialogueManager.Instance.SetDialogue(actorManager.currentDialogue);
        }
    }
}