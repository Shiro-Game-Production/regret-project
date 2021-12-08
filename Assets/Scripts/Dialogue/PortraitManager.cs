using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class PortraitManager : MonoBehaviour
    {
        [SerializeField] private Image portraitImage;

        public void SetPortraitSprite(Sprite portraitSprite)
        {
            portraitImage.sprite = portraitSprite;
            portraitImage.SetNativeSize();
        }
    }
}