﻿﻿using System;
using System.Collections;
using UnityEngine;

namespace Effects
{
    public static class FadingEffect
    {
        private const float FADE_WAITING_TIME = 0.05f;
        private const float FADING_SPEED = 0.1f;

        /// <summary>
        /// Fade in effect by using canvas group alpha
        /// From invisible (0) to visible (1)
        /// </summary>
        /// <param name="canvasGroup">CanvasGroup component</param>
        /// <param name="fadingSpeed">Fading speed for alpha value</param>
        /// <param name="beforeEffect">Action before fade effect</param>
        /// <param name="afterEffect">Action after fade effect</param>
        /// <param name="fadeWaitingTime">Waiting time every loop</param>
        /// <returns>Wait for certain seconds</returns>
        public static IEnumerator FadeIn(CanvasGroup canvasGroup, 
            float fadeWaitingTime = FADE_WAITING_TIME, float fadingSpeed = FADING_SPEED,
            Action beforeEffect = null, Action afterEffect = null)
        {
            float oldAlpha = 0;
            const float newAlpha = 1;
            
            beforeEffect?.Invoke();
            
            while (oldAlpha < newAlpha)
            {
                float currentAlpha = canvasGroup.alpha;
                
                // Increase current alpha
                currentAlpha += fadingSpeed;
                canvasGroup.alpha = currentAlpha;
                
                // Update old alpha
                oldAlpha = currentAlpha;
                yield return new WaitForSeconds(fadeWaitingTime);
            }
            // Block the raycast
            canvasGroup.blocksRaycasts = true;

            afterEffect?.Invoke();
        }

        /// <summary>
        /// Fade out effect by using canvas group alpha
        /// From visible (1) to invisible (0)
        /// </summary>
        /// <param name="canvasGroup">CanvasGroup component</param>
        /// <param name="fadingSpeed">Fading speed for alpha value</param>
        /// <param name="beforeEffect">Action before fade effect</param>
        /// <param name="afterEffect">Action after fade effect</param>
        /// <param name="fadeWaitingTime">Waiting time every loop</param>
        /// <returns>Wait for certain seconds</returns>
        public static IEnumerator FadeOut(CanvasGroup canvasGroup, 
            float fadeWaitingTime = FADE_WAITING_TIME, float fadingSpeed = FADING_SPEED,
            Action beforeEffect = null, Action afterEffect = null)
        {
            float oldAlpha = 1;
            const float newAlpha = 0;
            
            beforeEffect?.Invoke();
            
            while (oldAlpha > newAlpha)
            {
                float currentAlpha = canvasGroup.alpha;
                
                // Decrease current alpha
                currentAlpha -= fadingSpeed;
                canvasGroup.alpha = currentAlpha;
                
                // Update old alpha
                oldAlpha = currentAlpha;
                yield return new WaitForSeconds(fadeWaitingTime);
            }
            // Don't block the raycast
            canvasGroup.blocksRaycasts = false;

            afterEffect?.Invoke();
        }
    }
}