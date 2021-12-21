using UnityEngine;
using UnityEngine.UI;

namespace Dialogue.Portrait
{
    public class DialoguePortrait : MonoBehaviour
    {
        [SerializeField] private Image portraitImage;

        public void SetPortraitSprite(Sprite portraitSprite)
        {
            portraitImage.sprite = portraitSprite;
            portraitImage.SetNativeSize();
        }
    }
}