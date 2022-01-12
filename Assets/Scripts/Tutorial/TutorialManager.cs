using UnityEngine;
using Player;
using Dialogue;

public class TutorialManager : MonoBehaviour
{
    public GameObject popUps;

    //Start is called before the first frame update
    private void Start()
    {
        Invoke(nameof(StartTutorial), 0.04f);
    }

    private void StartTutorial()
    {
        if(!DialogueManager.Instance.DialogueIsPlaying && 
            !PlayerMovement.Instance.Movement.IsWalking)
        {
            
            popUps.SetActive(true);
            Debug.Log("Tutorial Nyala");
        }

        else if(DialogueManager.Instance.DialogueIsPlaying || 
            PlayerMovement.Instance.Movement.IsWalking)
        {
            popUps.SetActive(false);
            Debug.Log("Tutorial Mati");
        }
    }
}
