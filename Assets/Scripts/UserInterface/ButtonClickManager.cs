using System;
using Effects;
using UnityEngine;

namespace UserInterface
{
    public class ButtonClickManager : MonoBehaviour
    {
        private enum SceneLocation {Home, Gameplay}

        [SerializeField] private SceneLocation sceneLocation = SceneLocation.Gameplay;

        [DrawIf("sceneLocation", SceneLocation.Home)]
        [SerializeField] private CanvasGroup exitPrompts;
        
        [DrawIf("sceneLocation", SceneLocation.Gameplay)]
        [SerializeField] private CanvasGroup pausePrompts;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                switch (sceneLocation)
                {
                    case SceneLocation.Home:
                        ShowPrompts(exitPrompts);
                        break;
                    case SceneLocation.Gameplay:
                        ShowPrompts(pausePrompts);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void ShowPrompts(CanvasGroup canvasGroup)
        {
            StartCoroutine(FadingEffect.FadeIn(canvasGroup));
        }

        public void HidePrompts(CanvasGroup canvasGroup)
        {
            StartCoroutine(FadingEffect.FadeOut(canvasGroup));
        }
        
        public void ExitApplication()
        {
            Application.Quit();
        }
    }
}
