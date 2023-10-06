using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger: MonoBehaviour
{
    [SerializeField] DialogueContainer dialogueContainer;
    bool used = false;


	public void RequestDialogue()
    {
        DialogueManager.instance.RequestDialogue(dialogueContainer);
        used = true;
    }
}
