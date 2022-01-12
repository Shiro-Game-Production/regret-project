using Effects;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue.Illustration
{
    public class DialogueIllustration : MonoBehaviour
    {
        [SerializeField] private Image illustrationImage;

        /// <summary>
        /// Set illustrtation sprite
        /// </summary>
        /// <param name="illustrationSprite">Illustrtation  sprite</param>
        public void SetIllustration(Sprite illustrationSprite)
        {
            illustrationImage.sprite = illustrationSprite;
            illustrationImage.SetNativeSize();
            StartCoroutine(FadingEffect.FadeIn(illustrationImage));
        }
    }
}