using UnityEngine;
using Player;
using Dialogue;
using System;
using UnityEngine.UI;
using Effects;

namespace Tutorial{
    public class TutorialManager : SingletonBaseClass<TutorialManager>
    {
        [SerializeField] private TutorialData[] tutorialDatas;

        [Header("Parameters")]
        [Range(3, 10)]
        [SerializeField] private float popUpDuration = 5f;
        private bool showPopUp = true;
        private int popUpIndex;

        [Header("Tutorial UI")]
        [SerializeField] private Text tutorialText;
        [SerializeField] private CanvasGroup popUpTutorial;


        //Start is called before the first frame update
        private void Update()
        {
            Invoke(nameof(TutorialFlow), 0.04f);
        }

        /// <summary>
        /// Tutorial flow from the first tutorial until the last
        /// </summary>
        private void TutorialFlow(){
            switch(popUpIndex){
                case 0:
                    CheckTutorial(
                        !DialogueManager.Instance.DialogueIsPlaying && 
                        !PlayerMovement.Instance.Movement.IsWalking,
                        PlayerMovement.Instance.Movement.IsWalking);
                    break;
                default:
                    // Tutorial finish or crash
                    Debug.Log("Tutorial done");
                    Destroy(popUpTutorial.gameObject);
                    Destroy(gameObject);
                    break;
            }
        }

        /// <summary>
        /// Check tutorial if completed
        /// </summary>
        /// 
        /// <param name="condition">Tutorial requirements</param>
        private void CheckTutorial(bool requirements, bool condition)
        {
            if(requirements)
                ShowTutorial();

            if (condition)
                ShowNextTutorial();
        }

        /// <summary>
        /// Show next tutorial by:
        /// 1. Make showPopUp to true
        /// 1. Increasing pop up index
        /// </summary>
        private void ShowNextTutorial()
        {
            showPopUp = true;
            popUpIndex++;
        }

        /// <summary>
        /// SHow tutorial pop up text and set active all the objects
        /// </summary>
        private void ShowTutorial()
        {
            if (!showPopUp) return;
            
            ShowTutorialPopUp(tutorialDatas[popUpIndex].tutorialPopUp);
            
            showPopUp = false;
        }

        /// <summary>
        /// Show tutorial pop up
        /// </summary>
        /// <param name="tutorialText">Tutorial text</param>
        private void ShowTutorialPopUp(string tutorialText)
        {
            this.tutorialText.text = tutorialText;
            StartCoroutine(FadingEffect.FadeIn(popUpTutorial,
                afterEffect: () => Invoke(nameof(HideTutorialPopUp), popUpDuration))
            );
        }

        private void HideTutorialPopUp(){
            StartCoroutine(FadingEffect.FadeOut(popUpTutorial));
        }
    }

    [Serializable]
    public class TutorialData
    {
        public string name;
        [TextArea(3, 5)]
        public string tutorialPopUp;
    }
}