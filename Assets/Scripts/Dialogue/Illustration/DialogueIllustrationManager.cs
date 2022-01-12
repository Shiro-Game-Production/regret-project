using System.Collections.Generic;
using UnityEngine;
using Dialogue.Tags;

namespace Dialogue.Illustration{
    public class DialogueIllustrationManager : SingletonBaseClass<DialogueIllustrationManager> {

        [Header("Dialogue Illustrtation ")]
        [SerializeField] private Transform illustrtationsParent;
        [SerializeField] private DialogueIllustration illustrtationPrefab;

        private List<DialogueIllustration> illustrtationPool;

        private void Awake() {
            illustrtationPool = new List<DialogueIllustration>();
        }

        /// <summary>
        /// Display illustrtations
        /// </summary>
        /// <param name="filenames"></param>
        public void DisplayIllustrtations(string filenames)
        {
            string[] files = filenames.Split(',');
            
            // If there are no illustrtations or none, hide illustrtation and return right away
            if (files.Length <= 0 || filenames == DialogueTags.BLANK_VALUE)
            {
                HideIllustrtations();
                return;
            }
            
            HideIllustrtations(); // Hide previous illustrtation

            foreach (string filename in files)
            {
                Sprite illustration = Resources.Load<Sprite>($"Portraits/{filename}");
                DialogueIllustration illustrtationManager = GetOrCreateIllustrtationManager();

                illustrtationManager.gameObject.SetActive(true);
                illustrtationManager.SetIllustration(illustration);
            }
        }
        
        /// <summary>
        /// Hide illustrtation if there are no illustrtation
        /// </summary>
        public void HideIllustrtations()
        {
            foreach (DialogueIllustration illustrtation in illustrtationPool)
            {
                illustrtation.gameObject.SetActive(false);
            }
        }
        
        /// <summary>
        /// Illustrtation manager object pooling
        /// </summary>
        /// <returns>Return existing illustrtation manager in hierarchy or create a new one</returns>
        private DialogueIllustration GetOrCreateIllustrtationManager()
        {
            DialogueIllustration illustrationManager = illustrtationPool.Find(illustrtation => 
                !illustrtation.gameObject.activeInHierarchy);

            if (illustrationManager == null)
            {
                illustrationManager = Instantiate(illustrtationPrefab, illustrtationsParent).GetComponent<DialogueIllustration>();
                // Add new choice manager to pool 
                illustrtationPool.Add(illustrationManager);
            }
            
            illustrationManager.gameObject.SetActive(false);

            return illustrationManager;
        }
    }
}