using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DisplayCode : MonoBehaviour
{
    [SerializeField]
    private Text codeText;
    private string codeTextValue = "";

    private const string CORRECT_CODE = "284031"; 

    // Update is called once per frame
    private void Start()
    {
        codeText.text = "";
    }

    public void AddDigit(string digit)
    {
        if(codeText.text.Length < CORRECT_CODE.Length)
        {
            codeTextValue += digit;

            codeText.text = codeTextValue;
        }
    }

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

    public void DeleteCode()
    {
        int lastIndex = codeText.text.Length - 1; 

        codeTextValue = codeText.text.Remove(lastIndex);

        codeText.text = codeTextValue;
    }

}
