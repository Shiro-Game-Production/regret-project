using UnityEngine;
using UnityEngine.UI;

namespace Dialogue.Choices
{
    public class DialogueChoice : MonoBehaviour
    {
        [SerializeField] private Button choiceButton;
        [SerializeField] private Text choiceText;

        public int choiceIndex;
        
        public Text ChoiceText => choiceText;

        private void Start()
        {
            choiceButton.onClick.RemoveAllListeners();
            choiceButton.onClick.AddListener(() =>
            {
                DialogueChoiceManager.Instance.Decide(choiceIndex);
            });
        }
    }
}