using UnityEngine;
using UnityEngine.UI;

namespace Puzzle.Safe {
    public class SafeManager : MonoBehaviour
    {
        [SerializeField] private Text safeDisplayText;

        private const string CORRECT_CODE = "284031"; 

        private void Start()
        {
            safeDisplayText.text = "";
        }

        /// <summary>
        /// Add digit to code text
        /// </summary>
        /// <param name="digit"></param>
        public void AddDigit(string digit)
        {
            if(safeDisplayText.text.Length < CORRECT_CODE.Length)
            {
                safeDisplayText.text += digit;
            }
        }

        /// <summary>
        /// Check code in code text
        /// </summary>
        public void CheckCode()
        {
            if(safeDisplayText.text == CORRECT_CODE)
            {
                Debug.Log("Safe opened!");
            }
            else
            {
                Debug.Log("Kode Salah!");
            }
        }

        /// <summary>
        /// Delete number in code text
        /// </summary>
        public void DeleteCode()
        {
            string codeTextValue = safeDisplayText.text;
            // Return if there are no digits in code text
            if(codeTextValue.Length == 0) return;

            // Remove last index in code text
            int lastIndex = codeTextValue.Length - 1; 
            safeDisplayText.text = codeTextValue.Remove(lastIndex);
        }
    }
}