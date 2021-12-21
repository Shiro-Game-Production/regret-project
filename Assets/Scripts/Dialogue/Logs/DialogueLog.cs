using UnityEngine;
using UnityEngine.UI;

namespace Dialogue.Logs
{
    public class DialogueLog : MonoBehaviour
    {
        [SerializeField] private Text speakerName;
        [SerializeField] private Text dialogueText;

        /// <summary>
        /// Set dialogue log with speaker name and dialogue text
        /// </summary>
        /// <param name="speakerName">Speaker name</param>
        /// <param name="dialogueText">Dialogue text</param>
        public void SetDialogueLog(string speakerName, string dialogueText){
            this.speakerName.text = speakerName;
            this.dialogueText.text = dialogueText;
        }
    }
}