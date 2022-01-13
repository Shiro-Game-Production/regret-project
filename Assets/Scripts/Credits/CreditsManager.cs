using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Credits
{
    public class CreditsManager : MonoBehaviour
    {
        [SerializeField] private TextAsset creditsCsv;

        private Dictionary<string, string> creditsDict;
        private Dictionary<int, string> creditsHeaderDict;

        private void Awake() {
            creditsDict = new Dictionary<string, string>();
            creditsHeaderDict = new Dictionary<int, string>();
            ReadFile();
        }

        private void ReadFile(){
            // Get asset path
            string creditsPath = AssetDatabase.GetAssetPath(creditsCsv);
            
            // Check exist
            if(string.IsNullOrWhiteSpace(creditsPath) || 
                !File.Exists(creditsPath)) {
                Debug.LogError($"{creditsCsv} cannot be found");
                return;
            }

            StreamReader reader = new StreamReader(creditsPath);
            string lines = reader.ReadToEnd();
            reader.Close();

            ReadLines(lines);
        }

        private void ReadLines(string lines){
            string[] rows = lines.Split('\n');

            // Assign names
            for (int i = 0; i < rows.Length; i++)
            {
                string[] items = rows[i].Split(',');

                // Get header
                if(i == 0){
                    for (int j = 0; j < items.Length; j++)
                    {
                        string item = items[j].Trim();
                        Debug.Log($"Header: {item}");
                        if (!creditsDict.ContainsKey(item)){
                            creditsDict[item] = "";
                            creditsHeaderDict[j] = item;
                        }
                    }
                    continue;
                }

                for (int j = 0; j < items.Length; j++){
                    // If null or white space, continue
                    if(string.IsNullOrWhiteSpace(items[j])) continue;

                    Debug.Log($"Header: {items[j]}");
                    // Add names in dict
                    creditsDict[creditsHeaderDict[j]] += $"{items[j]}\n";
                }
            }

            foreach(string key in creditsDict.Keys){
                Debug.Log($"{key}: {creditsDict[key]}");
            }
        }
    }
}