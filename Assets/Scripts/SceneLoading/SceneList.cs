using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

namespace SceneLoading{
    public static class SceneList{
        private const string SCENE_NAME_PATTERN = @"Assets\/Scenes\/([A-Za-z0-9]+).unity";
        public static List<string> SceneNames(){
            List<string> sceneNames = new List<string>();
            Regex regexPattern = new Regex(SCENE_NAME_PATTERN);

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++){
                Match regexMatch = regexPattern.Match(SceneUtility.GetScenePathByBuildIndex(i));
                sceneNames.Add(regexMatch.Groups[1].Value);
            }

            return sceneNames;
        }
    }
}