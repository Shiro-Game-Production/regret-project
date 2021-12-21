using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Portrait{
    public class DialoguePortraitManager : SingletonBaseClass<DialoguePortraitManager> {

        [Header("Dialogue Portrait")]
        [SerializeField] private Transform portraitsParent;
        [SerializeField] private DialoguePortrait portraitPrefab;
        private List<DialoguePortrait> portraitPool;

        private void Awake() {
            portraitPool = new List<DialoguePortrait>();
        }

        /// <summary>
        /// Display portraits
        /// </summary>
        /// <param name="filenames"></param>
        public void DisplayPortraits(string filenames)
        {
            string[] files = filenames.Split(',');
            
            // If there are no portraits or none, hide portrait and return right away
            if (files.Length <= 0 || filenames == DialogueTags.BLANK_VALUE)
            {
                HidePortraits();
                return;
            }
            
            HidePortraits(); // Hide previous portrait

            foreach (string filename in files)
            {
                Sprite portrait = Resources.Load<Sprite>($"Portraits/{filename}");
                DialoguePortrait portraitManager = GetOrCreatePortraitManager();
                
                portraitManager.gameObject.SetActive(true);
                portraitManager.SetPortraitSprite(portrait);
            }
        }
        
        /// <summary>
        /// Hide portraits if there are no portraits
        /// </summary>
        public void HidePortraits()
        {
            foreach (DialoguePortrait portrait in portraitPool)
            {
                portrait.gameObject.SetActive(false);
            }
        }
        
        /// <summary>
        /// Portrait manager object pooling
        /// </summary>
        /// <returns>Return existing portrait manager in hierarchy or create a new one</returns>
        private DialoguePortrait GetOrCreatePortraitManager()
        {
            DialoguePortrait portraitManager = portraitPool.Find(portrait => !portrait.gameObject.activeInHierarchy);

            if (portraitManager == null)
            {
                portraitManager = Instantiate(portraitPrefab, portraitsParent).GetComponent<DialoguePortrait>();
                // Add new choice manager to pool 
                portraitPool.Add(portraitManager);
            }
            
            portraitManager.gameObject.SetActive(false);

            return portraitManager;
        }
    }
}