using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChoice : MonoBehaviour
{
    public string choiceGUID;
    [SerializeField] TMPro.TextMeshProUGUI textGUI;
    [SerializeField] AudioClip clip;

    public void Initialize(string guid,string text) 
    {
        choiceGUID = guid;
        FilterText(text);// make this prettier.
    }

    public void FilterText(string text)
    {
        if (text.Equals("Choice 1"))
            textGUI.text = "Continue";
        else
            textGUI.text = text;
    }

    public void DestroySelf()
    {
        try
        {
            Destroy(gameObject);
        }
        catch { }
    }

    public void Choose()
    {
        if (choiceGUID.Equals("END DIALOGUE"))
        {
            DialogueManager.instance.StopDialogue();
            Destroy(gameObject);
        }
        else
            DialogueManager.instance.IncrementDialogue(choiceGUID);
    }
}
