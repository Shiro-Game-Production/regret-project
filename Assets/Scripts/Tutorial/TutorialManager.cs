using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Dialogue;

public class TutorialManager : MonoBehaviour
{
    public GameObject popUps;

    private bool hasShown = false;

    //Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("StartTutorial", 0.04f);
        
        //StopTutorial();
    }

    void StartTutorial()
    {
        if(!DialogueManager.Instance.DialogueIsPlaying && !PlayerMovement.Instance.Movement.IsWalking && !hasShown)
        {
            
            popUps.SetActive(true);
            hasShown = true;
            Debug.Log("Tutorial Nyala");
        }

        else if(DialogueManager.Instance.DialogueIsPlaying || PlayerMovement.Instance.Movement.IsWalking)
        {
            popUps.SetActive(false);
            Debug.Log("Tutorial Mati");
        }
    }

    // void StopTutorial()
    // {
    //     if(DialogueManager.Instance.DialogueIsPlaying || PlayerMovement.Instance.IsWalking)
    //     {
    //         popUps.SetActive(false);
    //         Debug.Log("Tutorial Mati");
    //     }
    // }
}
