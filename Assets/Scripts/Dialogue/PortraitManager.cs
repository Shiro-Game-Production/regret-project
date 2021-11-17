using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class PortraitManager : MonoBehaviour
    {
        [SerializeField] private Image portraitImage;

        public Image PortraitImage => portraitImage;

        private void Start()
        {
            portraitImage.SetNativeSize();
        }
    }
}