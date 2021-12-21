using UnityEngine;

namespace Dialogue.Tags
{
    public static class DialogueTags
    {
        [Header("Tags")]
        public const string AUDIO_TAG = "audio";
        public const string DIALOGUE_BOX_TAG = "dialogue-box";
        public const string EFFECT_TAG = "effect";
        public const string ENDING_TAG = "end";
        public const string EVENT_TAG = "event";
        public const string PORTRAIT_TAG = "portrait";
        public const string SPEAKER_TAG = "speaker";

        [Header("Tag Value")]
        public const string BLANK_VALUE = "none";
        [Header("Tag: dialogue-box")]
        public const string SHOW_DIALOGUE_BOX = "show";
        [Header("Tag: effect")]
        public const string SHAKE_TAG = "shake";
        [Header("Tag: ending")]
        public const string CONFIRM_ENDING = "true";
    }
}