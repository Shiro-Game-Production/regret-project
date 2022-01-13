using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Credits
{
    public class CreditsManager : SingletonBaseClass<CreditsManager>
    {
        public Sprite creditBackgroundSprite;

        private List<Credit> creditList;

        public List<Credit> CreditList => creditList;

        private const string CREDITS_PATH = "Assets/Resources/credits.tsv";

        /// <summary>
        /// Set instance and don't destroy on load
        /// </summary>
        private void SetInstance()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Awake()
        {
            SetInstance();
            creditList = new List<Credit>();
            ReadFile();
        }

        /// <summary>
        /// Read file CSV or TSV
        /// </summary>
        private void ReadFile()
        {
            // Check path existance
            if (!File.Exists(CREDITS_PATH))
            {
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
        private void ReadLines(string lines)
        {
            string[] rows = lines.Split('\n');

            // Assign names
            for (int i = 0; i < rows.Length; i++)
            {
                string[] items = rows[i].Split('\t');

                // Get header
                if (i == 0)
                {
                    for (int j = 0; j < items.Length; j++)
                    {
                        string item = items[j].Trim();
                        creditList.Add(new Credit(j, item, ""));
                    }
                    continue;
                }

                // Get values
                for (int j = 0; j < items.Length; j++)
                {
                    // If null or white space, continue
                    if (string.IsNullOrWhiteSpace(items[j])) continue;
                    // Add names in dict
                    creditList[j].body += $"{items[j]}\n";
                }
            }
        }
    }

    public class Credit{
        private readonly int id;
        private readonly string title;
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