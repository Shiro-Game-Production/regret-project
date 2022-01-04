using System;
using System.Collections;
using UnityEngine;

namespace Effects{
    public static class AudioFadingEffect{
        private const float FADE_WAITING_TIME = 0.05f;
        private const float FADING_SPEED = 0.01f;

        public static IEnumerator FadeIn(AudioSource audioSource, 
            float newVolume,
            float fadeWaitingTime = FADE_WAITING_TIME, float fadingSpeed = FADING_SPEED, 
            Action beforeEffect = null, Action afterEffect = null) 
        {
            float oldVolume = audioSource.volume;

            beforeEffect?.Invoke();

            while(oldVolume < newVolume){
                float currentVolume = audioSource.volume;

                // Increase the volume 
                currentVolume += fadingSpeed;
                audioSource.volume = currentVolume;

                oldVolume = currentVolume;
                yield return new WaitForSeconds(fadeWaitingTime);
            }

            afterEffect?.Invoke();
        }

        public static IEnumerator FadeOut(AudioSource audioSource, 
            float fadeWaitingTime = FADE_WAITING_TIME, float fadingSpeed = FADING_SPEED, 
            Action beforeEffect = null, Action afterEffect = null) 
        {
            float oldVolume = audioSource.volume;
            const float newVolume = 0;

            while(oldVolume > newVolume){
                float currentVolume = audioSource.volume;

                // Increase the volume 
                currentVolume -= fadingSpeed;
                audioSource.volume = currentVolume;

                oldVolume = currentVolume;
                yield return new WaitForSeconds(fadeWaitingTime);
            }

            afterEffect?.Invoke();
        }
    }
}