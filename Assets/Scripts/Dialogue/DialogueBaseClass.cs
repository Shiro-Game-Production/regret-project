using Dialogue.Choices;
using Dialogue.Logs;
using Dialogue.Portrait;
using Dialogue.Tags;
using UnityEngine;

namespace Dialogue{
    public class DialogueBaseClass : SingletonBaseClass<DialogueBaseClass> {
        protected DialogueChoiceManager dialogueChoiceManager;
        protected DialogueLogManager dialogueLogManager;
        protected DialoguePortraitManager dialoguePortraitManager;
        protected DialogueTagManager dialogueTagsManager;
    }
}