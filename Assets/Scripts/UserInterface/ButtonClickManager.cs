using System;
using Effects;
using SceneLoading;
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
                        ShowPrompts(pausePrompts, () => PauseOrResumeGame(true));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        /// <summary>
        /// Pause or resume game
        /// </summary>
        /// <param name="pauseGame">True to pause, False to resume</param>
        public void PauseOrResumeGame(bool pauseGame)
        {
            Time.timeScale = pauseGame ? 0f : 1f;
        }
        
        public void ShowPrompts(CanvasGroup canvasGroup)
        {
            StartCoroutine(FadingEffect.FadeIn(canvasGroup));
        }

        public void ShowPrompts(CanvasGroup canvasGroup, Action afterEffect = null)
        {
            StartCoroutine(FadingEffect.FadeIn(canvasGroup, afterEffect: afterEffect));
        }

        public void HidePrompts(CanvasGroup canvasGroup)
        {
            StartCoroutine(FadingEffect.FadeOut(canvasGroup));
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
