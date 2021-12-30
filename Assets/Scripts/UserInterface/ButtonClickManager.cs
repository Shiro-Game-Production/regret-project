using Effects;
using SceneLoading;
using UnityEngine;

namespace UserInterface
{
    public class ButtonClickManager : SingletonBaseClass<ButtonClickManager>
    {
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
