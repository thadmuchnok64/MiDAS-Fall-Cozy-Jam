using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Customer", menuName = "Customer", order = 1)]
public class Customer : ScriptableObject
{
    public string name;
    public Sprite icon;
    public DialogueContainer dialogueContainer;
    public DialogueContainer happyDialogueContainer;
    public DialogueContainer sadDialogueContainer;
    public string correctItemName;
    public bool customerSatisfied = false;
}
