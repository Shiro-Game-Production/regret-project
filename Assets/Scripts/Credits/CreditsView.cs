using System.Collections;
using System.Collections.Generic;
using Effects;
using SceneLoading;
using UnityEngine;
using UnityEngine.UI;

namespace Credits{
    public class CreditsView : MonoBehaviour {
        [Range(0, 10)]
        [SerializeField] private float creditDuration = 3f;
        [Range(0, 5)]
        [SerializeField] private float betweenCreditDuration = 1f;

        [Header("Credit UI")]
        [SerializeField] private Image creditBackgroundImage;
        [SerializeField] private CanvasGroup creditCanvasGroup;
        [SerializeField] private Text titleText, bodyText;

        private void Start() {
            SetCreditImage();
            StartCoroutine(AnimateCredits(CreditsManager.Instance.CreditList));
        }

        private void SetCreditImage(){
            creditBackgroundImage.sprite = CreditsManager.Instance.creditBackgroundSprite;
            creditBackgroundImage.SetNativeSize();
        }
        
        /// <summary>
        /// Animate credits
        /// </summary>
        /// <returns></returns>
        private IEnumerator AnimateCredits(List<Credit> creditList)
        {
            // Loop through credits
            for (int i = 0; i < creditList.Count; i++)
            {
                Credit credit = creditList[i];

                // Fade in
                StartCoroutine(FadingEffect.FadeIn(creditCanvasGroup,
                    beforeEffect: () =>
                    {
                        titleText.text = credit.Title;
                        bodyText.text = credit.body;
                    })
                );
                // Wait
                yield return new WaitForSeconds(creditDuration);

                // Fade out
                StartCoroutine(FadingEffect.FadeOut(creditCanvasGroup,
                    afterEffect: () =>
                    {
                        titleText.text = "";
                        bodyText.text = "";
                    })
                );

                yield return new WaitForSeconds(betweenCreditDuration);
            }

            SceneLoadTrigger.Instance.LoadScene("HomeScene");
        }
    }
}