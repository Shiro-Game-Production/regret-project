using UnityEngine;

namespace Dialogue
{
    public class BeginChapterTrigger : MonoBehaviour
    {
        [SerializeField] private TextAsset initialTextAsset;
        private DialogueManager dialogueManager;

        private void Awake()
        {
            dialogueManager = DialogueManager.Instance;
        }

        private void Start()
        {
            // Start chapter after 0.02s
            Invoke(nameof(StartChapter), 0.02f);
        }
        
        /// <summary>
        /// Start chapter by setting dialogue to dialogue manager
        /// </summary>
        private void StartChapter()
        {
            dialogueManager.gameObject.SetActive(true);
            dialogueManager.SetDialogue(initialTextAsset);
        }
    }
}
