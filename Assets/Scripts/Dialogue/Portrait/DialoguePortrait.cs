using UnityEngine;
using UnityEngine.UI;

namespace Dialogue.Portrait
{
    public class DialoguePortrait : MonoBehaviour
    {
        [SerializeField] private string portraitName;
        [SerializeField] private Image portraitImage;

        public string PortraitName => portraitName;

        /// <summary>
        /// Set portrait sprite and name
        /// </summary>
        /// <param name="portraitSprite">Portrait sprite</param>
        /// <param name="portraitName">Portrait name</param>
        public void SetPortrait(Sprite portraitSprite, string portraitName)
        {
            this.portraitName = portraitName;
            portraitImage.sprite = portraitSprite;
            portraitImage.SetNativeSize();
        }
    }
}