using System.Collections.Generic;
using Audios.SoundEffects;
using Event.FinishConditionScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle.Safe {
    public class SafeManager : PuzzleManager
    {
        [SerializeField] private Text safeDisplayText;

        [SerializeField] private string correctCode = "251299";

        [Header("Audio")]
        [SerializeField] private List<AudioClip> keypadAudios;

        private SoundEffectManager soundEffectManager;

        private void Awake()
        {
            safeDisplayText.text = "";
            soundEffectManager = SoundEffectManager.Instance;
        }

        /// <summary>
        /// Add digit to code text
        /// </summary>
        /// <param name="digit"></param>
        public void AddDigit(string digit)
        {
            PlayKeypadAudio();
            if(safeDisplayText.text.Length < correctCode.Length)
            {
                safeDisplayText.text += digit;
            }
        }

        /// <summary>
        /// Check code in code text
        /// </summary>
        public void CheckCode()
        {
            if(safeDisplayText.text == correctCode)
            {
                soundEffectManager.Play(ListSoundEffect.SafeOpened);
                puzzleFinishedCondition.OnEndingCondition();
            }
            else
            {
                soundEffectManager.Play(ListSoundEffect.AccessDenied);
            }
        }

        /// <summary>
        /// Delete number in code text
        /// </summary>
        public void DeleteCode()
        {
            PlayKeypadAudio();
            string codeTextValue = safeDisplayText.text;
            // Return if there are no digits in code text
            if(codeTextValue.Length == 0) return;

            // Remove last index in code text
            int lastIndex = codeTextValue.Length - 1; 
            safeDisplayText.text = codeTextValue.Remove(lastIndex);
        }

        private void PlayKeypadAudio(){
            int randomAudioIndex = Random.Range(0, keypadAudios.Count);
            soundEffectManager.Play(keypadAudios[randomAudioIndex]);
        }
    }
}