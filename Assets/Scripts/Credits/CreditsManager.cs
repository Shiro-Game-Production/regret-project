using System.Collections;
using System.Collections.Generic;
using System.IO;
using Effects;
using UnityEngine;
using UnityEngine.UI;

namespace Credits
{
    public class CreditsManager : MonoBehaviour
    {
        [Range(0, 10)]
        [SerializeField] private float creditDuration = 3f;
        [Range(0, 5)]
        [SerializeField] private float betweenCreditDuration = 1f;

        [Header("Credit UI")]
        [SerializeField] private CanvasGroup creditCanvasGroup;
        [SerializeField] private Text titleText, bodyText;

        private List<Credit> creditList;
        private const string CREDITS_PATH = "Assets/Resources/credits.tsv";

        private void Awake() {
            creditList = new List<Credit>();
            ReadFile();
        }
        
        /// <summary>
        /// Read file CSV or TSV
        /// </summary>
        private void ReadFile(){
            // Check path existance
            if(!File.Exists(CREDITS_PATH)) {
                Debug.LogError($"{CREDITS_PATH} cannot be found");
                return;
            }

            StreamReader reader = new StreamReader(CREDITS_PATH);
            string lines = reader.ReadToEnd();
            reader.Close();

            ReadLines(lines);
        }

        /// <summary>
        /// Read lines in files and convert it to dictionary
        /// </summary>
        /// <param name="lines"></param>
        private void ReadLines(string lines){
            string[] rows = lines.Split('\n');

            // Assign names
            for (int i = 0; i < rows.Length; i++)
            {
                string[] items = rows[i].Split('\t');

                // Get header
                if(i == 0){
                    for (int j = 0; j < items.Length; j++)
                    {
                        string item = items[j].Trim();
                        creditList.Add(new Credit(j, item, ""));
                    }
                    continue;
                }

                // Get values
                for (int j = 0; j < items.Length; j++){
                    // If null or white space, continue
                    if(string.IsNullOrWhiteSpace(items[j])) continue;
                    // Add names in dict
                    creditList[j].body += $"{items[j]}\n";
                }
            }

            StartCoroutine(AnimateCredits());
        }

        private IEnumerator AnimateCredits(){
            // Loop through credits
            for (int i = 0; i < creditList.Count; i++)
            {
                Credit credit = creditList[i];

                // Fade in
                StartCoroutine(FadingEffect.FadeIn(creditCanvasGroup, 
                    beforeEffect: () => {
                        titleText.text = credit.Title;
                        bodyText.text = credit.body;
                    })
                );
                // Wait
                yield return new WaitForSeconds(creditDuration);

                // Fade out
                StartCoroutine(FadingEffect.FadeOut(creditCanvasGroup, 
                    afterEffect: () => {
                        titleText.text = "";
                        bodyText.text = "";
                    })
                );

                yield return new WaitForSeconds(betweenCreditDuration);
            }
        }
    }

    public class Credit{
        private int id;
        private string title;
        public string body;

        public Credit(int id, string title, string body){
            this.id = id;
            this.title = title;
            this.body = body;
        }

        public int ID => id;
        public string Title => title;
    }
}