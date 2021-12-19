using UnityEngine;
using UnityEngine.UI;

namespace Puzzle.Safe {
    public class DisplayCode : MonoBehaviour
    {
        [SerializeField] private Text codeText;

        private string codeTextValue = "";
        private const string CORRECT_CODE = "284031"; 

        private void Start()
        {
            codeText.text = "";
        }

        /// <summary>
        /// Add digit to code text
        /// </summary>
        /// <param name="digit"></param>
        public void AddDigit(string digit)
        {
            if(codeText.text.Length < CORRECT_CODE.Length)
            {
                codeTextValue += digit;
                codeText.text = codeTextValue;
            }
        }

        /// <summary>
        /// Check code in code text
        /// </summary>
        public void CheckCode()
        {
            if(codeTextValue == CORRECT_CODE)
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
            int lastIndex = codeText.text.Length - 1; 
            codeTextValue = codeText.text.Remove(lastIndex);
            codeText.text = codeTextValue;
        }

    }
}