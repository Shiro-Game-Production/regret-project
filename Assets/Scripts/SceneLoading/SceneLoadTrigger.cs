using Credits;
using Effects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneLoading
{
    public class SceneLoadTrigger : SingletonBaseClass<SceneLoadTrigger>
    {
        [SerializeField] private Image fadingImage;

        [Header("Credit Background")]
        [SerializeField] private Sprite homeSceneCreditSprite;
        [SerializeField] private Sprite finalSceneCreditSprite;

        private const string CREDIT_SCENE_NAME = "CreditScene";
        private const string HOME_SCENE_NAME = "HomeScene";
        private const string FINAL_SCENE_NAME = "Chapter1";

        public Image FadingImage => fadingImage;

        #region Don't Destroy On Load

        /// <summary>
        /// Use only 1 Scene Load Trigger from HomeScene
        /// </summary>
        private void SetInstance()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        #endregion
        
        private const string LOADING_SCENE_NAME = "LoadingScene";

        private void Awake()
        {
            SetInstance();

            SceneManager.activeSceneChanged += ChangeActiveScene;
        }

        /// <summary>
        /// Change active scene
        /// </summary>
        /// <param name="current"></param>
        /// <param name="next"></param>
        private void ChangeActiveScene(Scene current, Scene next){
            StartCoroutine(FadingEffect.FadeOut(fadingImage));
        }

        /// <summary>
        /// Load loading scene to trigger scene loader by name
        /// </summary>
        /// <param name="sceneName">Scene's name to load</param>
        public void LoadScene(string sceneName)
        {
            // If next scene is credits, ...
            if(sceneName == CREDIT_SCENE_NAME){
                // Change background according to each scene's sprite
                switch(SceneManager.GetActiveScene().name){
                    case HOME_SCENE_NAME:
                        CreditsManager.Instance.creditBackgroundSprite = homeSceneCreditSprite;
                        break;
                    case FINAL_SCENE_NAME:
                        CreditsManager.Instance.creditBackgroundSprite = finalSceneCreditSprite;
                        break;
                }
            }

            StartCoroutine(FadingEffect.FadeIn(fadingImage,
                afterEffect: () =>
                {
                    LoadingData.sceneName = sceneName;
                    SceneManager.LoadScene(LOADING_SCENE_NAME);
                })
            );
        }
    }
}