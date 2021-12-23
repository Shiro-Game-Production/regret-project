using System.Collections.Generic;
using System.Linq;
using Audios;
using Dialogue.Portrait;
using Dialogue.Logs;
using Effects;
using Event;
using GameCamera;
using SceneLoading;
using UnityEngine;
using UnityEngine.UI;
using Event.DialogueEvent;

namespace Dialogue.Tags{
    public class DialogueTagManager : SingletonBaseClass<DialogueTagManager> {
        [Header("Dialogue UI")]
        [SerializeField] private GameObject dialogueTextBox;
        
        [Header("Ending")]
        [SerializeField] private Image blackScreen;

        [Header("Event Data")]
        [SerializeField] private List<EventData> eventDatas;

        private CameraShake cameraShake;
        private DialogueManager dialogueManager;
        private DialogueLogManager dialogueLogManager;
        private DialoguePortraitManager dialoguePortraitManager;
        private DialogueEventManager eventManager;

        private void Awake() {
            cameraShake = CameraShake.Instance;
            dialogueManager = DialogueManager.Instance;
            dialogueLogManager = DialogueLogManager.Instance;
            dialoguePortraitManager = DialoguePortraitManager.Instance;
            eventManager = DialogueEventManager.Instance;
        }

        /// <summary>
        /// Handle tags in dialogue
        /// </summary>
        /// <param name="dialogueTags"></param>
        public void HandleTags(List<string> dialogueTags)
        {
            foreach (string dialogueTag in dialogueTags)
            {
                // Parse the tag
                string[] splitTag = dialogueTag.Split(':');

                if (splitTag.Length != 2)
                {
                    Debug.LogError("Tag could not be parsed: " + tag);
                }

                string tagKey = splitTag[0].Trim();
                string tagValue = splitTag[1].Trim();
                
                // Handle tag
                switch (tagKey)
                {
                    case DialogueTags.AUDIO_TAG:
                        HandleAudio(tagValue);
                        break;
                    
                    case DialogueTags.DIALOGUE_BOX_TAG:
                        ShowOrHideDialogueBox(tagValue);
                        break;

                    case DialogueTags.EFFECT_TAG:
                        switch (tagValue)
                        {
                            case DialogueTags.SHAKE_TAG:
                                StartCoroutine(cameraShake.ShakingEffect());
                                break;
                        }
                        break;
                    
                    case DialogueTags.ENDING_TAG:
                        HandleEndingTag(tagValue);
                        break;
                    
                    case DialogueTags.EVENT_TAG:
                        SetEventData(tagValue);
                        break;
                    
                    case DialogueTags.PORTRAIT_TAG:
                        dialoguePortraitManager.DisplayPortraits(tagValue);
                        break;
                    
                    case DialogueTags.SPEAKER_TAG:
                        dialogueLogManager.speakerNameValue = tagValue == DialogueTags.BLANK_VALUE ? "" : tagValue;
                        dialogueManager.SpeakerName.text = dialogueLogManager.speakerNameValue;
                        break;
                    
                    default:
                        Debug.LogError("Tag is not in the list: " + tag);
                        break;
                }
            }
        }

        #region Audio
        
        private void HandleAudio(string audio)
        {
            AudioManager.Instance.Play(audio);
        }
        
        #endregion

        #region Dialogue Box

        /// <summary>
        /// Show or hide dialogue box depends on tag value
        /// </summary>
        /// <param name="tagValue"></param>
        private void ShowOrHideDialogueBox(string tagValue){
            switch(tagValue){
                case DialogueTags.BLANK_VALUE:
                    dialogueTextBox.SetActive(false);
                    break;
                
                case DialogueTags.SHOW_DIALOGUE_BOX:
                    dialogueTextBox.SetActive(true);
                    break;

                default:
                    Debug.LogError($"Tag: {tagValue} is not registered to handle dialogue box");
                    break;
            }
        }

        #endregion

        #region Event
        
        /// <summary>
        /// Find event data in list
        /// </summary>
        /// <param name="eventDataName">Event data name</param>
        private void SetEventData(string eventDataName)
        {
            // Find event data in list
            foreach (EventData eventData in eventDatas.Where(
                eventData => eventData.EventName == eventDataName))
            {
                eventManager.SetEventData(eventData);
                eventData.gameObject.SetActive(true);
                break;
            }
        }
        
        #endregion

        #region Ending
        
        /// <summary>
        /// Handle ending tag
        /// </summary>
        /// <param name="tagValue"></param>
        private void HandleEndingTag(string tagValue)
        {
            if (tagValue != DialogueTags.CONFIRM_ENDING) return;
            
            StartCoroutine(FadingEffect.FadeIn(blackScreen,
                fadingSpeed: 0.02f,
                afterEffect: () => SceneLoadTrigger.Instance.LoadScene("HomeScene"))
            );
        }

        #endregion
    }
}