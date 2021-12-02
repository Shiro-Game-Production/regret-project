using Effects;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class ItemNotificationManager : SingletonBaseClass<ItemNotificationManager>
    {
        [SerializeField] private Text notificationText;
        [SerializeField] private CanvasGroup notificationCanvasGroup;
        [SerializeField] private float notificationDuration = 3f;
        
        /// <summary>
        /// Set notification text
        /// Fade in and then after 3 seconds fade out
        /// </summary>
        /// <param name="notificationText">Notification text</param>
        public void SetNotificationText(string notificationText)
        {
            StartCoroutine(FadingEffect.FadeIn(
                notificationCanvasGroup,
                beforeEffect: () =>
                {
                    this.notificationText.text = notificationText;
                })
            );
            
            Invoke(nameof(HideNotification), notificationDuration);
        }
        
        /// <summary>
        /// Hide notification by fading out
        /// </summary>
        private void HideNotification()
        {
            StartCoroutine(FadingEffect.FadeOut(notificationCanvasGroup));
        }
    }
}