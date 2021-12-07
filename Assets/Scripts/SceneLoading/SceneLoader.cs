using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneLoading
{
    public class SceneLoader : MonoBehaviour
    {
        [Header("Rotation animation")]
        [SerializeField] private RectTransform loadingImage;
        [Range(100, 500)]
        [SerializeField] private float rotationSpeed = 350;
        
        [Header("Typing animation")]
        [SerializeField] private Text loadingText;
        [Range(0, 1)]
        [SerializeField] private float typingSpeed = 0.05f;
        private bool stopAnimating;
        
        private const float WAIT_SECONDS = 2.0f;
        
        private void Start()
        {
            StartCoroutine(TypingAnimation());
            StartCoroutine(LoadSceneAsync());
        }

        private void Update()
        {
            loadingImage.Rotate(Vector3.back * (Time.deltaTime * rotationSpeed));
        }
        
        /// <summary>
        /// Typing animation
        /// </summary>
        /// <returns></returns>
        private IEnumerator TypingAnimation()
        {
            while (!stopAnimating)
            {
                loadingText.text = "Loading";

                foreach (char letter in ".....")
                {
                    yield return new WaitForSeconds(typingSpeed);
                    loadingText.text += letter;
                }
            }
        }

        /// <summary>
        /// Load scene asynchronously
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadSceneAsync()
        {
            // Wait for 3 seconds
            yield return new WaitForSeconds(WAIT_SECONDS);
            
            // Load scene asynchronously
            AsyncOperation loadingScene = SceneManager.LoadSceneAsync(LoadingData.SceneName);

            stopAnimating = loadingScene.isDone;
        }
    }
}