using System;
using Dialogue;
using UnityEngine;

namespace Items
{
    public class ItemData: MonoBehaviour
    {
        public enum ItemMode { DialogueMode, NormalMode }
        public enum InteractionText { Interaksi, Buka, Bicara }

        [SerializeField] private string itemName;
        public InteractionText interactionText = InteractionText.Interaksi;
        public ItemMode itemMode = ItemMode.NormalMode;
        public TextAsset currentDialogue;

        public string ItemName => itemName;

        public virtual void HandleInteraction()
        {
            switch (itemMode)
            {
                case ItemMode.DialogueMode:
                    HandleDialogue();
                    break;
                case ItemMode.NormalMode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void HandleDialogue()
        {
            if(currentDialogue)
                DialogueManager.Instance.SetDialogue(currentDialogue);
        }
    }
}