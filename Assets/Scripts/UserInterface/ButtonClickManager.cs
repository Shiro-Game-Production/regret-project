using Effects;
using SceneLoading;
using UnityEngine;

namespace UserInterface
{
    public class ButtonClickManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup pauseCanvasGroup;
        
        /// <summary>
        /// Pause or resume game
        /// </summary>
        /// <param name="pauseGame">True to pause, False to resume</param>
        public void PauseGame(bool pauseGame)
        {
            if(pauseGame)
            {
                StartCoroutine(FadingEffect.FadeIn(pauseCanvasGroup, 
                    afterEffect: () => Time.timeScale = 0f)
                );
            }
            else
            {
                StartCoroutine(FadingEffect.FadeOut(pauseCanvasGroup, 
                    beforeEffect: () => Time.timeScale = 1f)
                );
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
