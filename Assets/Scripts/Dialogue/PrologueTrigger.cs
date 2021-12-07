using UnityEngine;

namespace Dialogue
{
    public class PrologueTrigger : MonoBehaviour
    {
        [SerializeField] private TextAsset prologueTextAsset;
        private DialogueManager dialogueManager;

        private void Awake()
        {
            dialogueManager = DialogueManager.Instance;
        }

        private void Start()
        {
            Invoke(nameof(SetPrologueTrigger), 0.02f);
        }
        
        /// <summary>
        /// Set prologue trigger after a certain seconds
        /// </summary>
        private void SetPrologueTrigger()
        {
            dialogueManager.gameObject.SetActive(true);
            dialogueManager.SetDialogue(prologueTextAsset);
        }
    }
}
