using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float typingSpeed = 0.04f;

        [Header("Dialogue UI")] 
        [SerializeField] private Text dialogueText;
        [SerializeField] private Text speakerName;
        [SerializeField] private Transform choicesParent;
        [SerializeField] private Transform portraitsParent;
    }
}
