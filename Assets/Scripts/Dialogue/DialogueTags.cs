﻿using UnityEngine;

namespace Dialogue
{
    public static class DialogueTags
    {
        [Header("Tags")]
        public const string AUDIO_TAG = "audio";
        public const string EFFECT_TAG = "effect";
        public const string EVENT_TAG = "event";
        public const string PORTRAIT_TAG = "portrait";
        public const string SPEAKER_TAG = "speaker";

        [Header("Tag Value")]
        public const string BLANK_VALUE = "none";
        [Header("Tag: effect")]
        public const string SHAKE_TAG = "shake";
    }
}