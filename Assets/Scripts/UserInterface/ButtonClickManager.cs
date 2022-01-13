using Effects;
using SceneLoading;
using UnityEngine;
using System;

namespace UserInterface
{
    public class ButtonClickManager : SingletonBaseClass<ButtonClickManager>
    {
        public void ShowPrompts(CanvasGroup canvasGroup)
        {
            StartCoroutine(FadingEffect.FadeIn(canvasGroup));
        }

        public void ShowPrompts(CanvasGroup canvasGroup, Action beforeEffect)
        {
            StartCoroutine(FadingEffect.FadeIn(canvasGroup, beforeEffect: beforeEffect));
        }

        public void HidePrompts(CanvasGroup canvasGroup)
        {
            StartCoroutine(FadingEffect.FadeOut(canvasGroup));
        }

        public void HidePrompts(CanvasGroup canvasGroup, Action afterEffect)
        {
            StartCoroutine(FadingEffect.FadeOut(canvasGroup, afterEffect: afterEffect));
        }

        public void LoadScene(string sceneName)
        {
            SceneLoadTrigger.Instance.LoadScene(sceneName);
        }
        
        public void ExitApplication()
        {
            Application.Quit();
        }
    }
}
