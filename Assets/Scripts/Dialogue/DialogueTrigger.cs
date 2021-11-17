using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private TextAsset dialogueJson;

        public TextAsset DialogueJson
        {
            get => dialogueJson;
            set => dialogueJson = value;
        }
    }
}