using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    [HideInInspector] public bool isOccupied = false;
    DialogueContainer dialogueContainer;
    string currentNode;
    [SerializeField] TMPro.TextMeshProUGUI dialogueBox;
    [SerializeField] GameObject button;
    [SerializeField] GameObject choicesBox;
    List<DialogueChoice> loadedChoices;
    [SerializeField] AudioClip dialogueCharacterSound;

    private void Start()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple dialogue managers!");
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        loadedChoices = new List<DialogueChoice>();
        //Invoke("TestDialogue", 1f);
    }
    

    public bool RequestDialogue(DialogueContainer containter,Sprite portrait)
    {
        if (isOccupied)
            return false;
        ClearChoices();
        isOccupied = true;
		dialogueBox.text = "";
		dialogueContainer = containter;
        currentNode = containter.NodeLinks[0].TargetNodeGUID;
        HUDManager.Instance.ToggleDialogue(true,portrait);
        Invoke("InitializeDialogue",1.25f);
        return true;
    }

    public void InitializeDialogue()
    {
		dialogueBox.text = "";
		IncrementDialogue(currentNode);

	}
	public void IncrementDialogue(string targetGUID)
    {
        currentNode = targetGUID;

        try
        {
            StartCoroutine(DispenseDialogue(dialogueContainer.DialogueNodeData.First(data => data.GUID == currentNode).DialogueText));
            GenerateChoices(targetGUID);
        }
        catch
        {
            StopDialogue();
        }
    }

    public IEnumerator DispenseDialogue(string dialogue)
    {
        string d = "";
        int i = 0;
        int randint = Random.Range(3, 7);
        foreach(char c in dialogue)
        {
            i++;
            d = d + c;
            dialogueBox.text = d;
            if (i % randint == 0)
            {
				 randint = Random.Range(3, 7);
			}
			yield return new WaitForEndOfFrame();
        }
        dialogueBox.text = d;
    }

    public void StopDialogue()
    {
        HUDManager.Instance.ToggleDialogue(false);
        isOccupied = false;
    }
    private void GenerateChoices(string targetGUID)
    {
        ClearChoices();
        var choices = dialogueContainer.NodeLinks.Where(link => link.BaseNodeGUID.Equals(targetGUID)).ToList();
        foreach (NodeLinkData choice in choices)
        {
            var choiceButton = Instantiate(button, choicesBox.transform.position, choicesBox.transform.rotation, choicesBox.transform).GetComponent<DialogueChoice>(); //problem
            choiceButton.Initialize(choice.TargetNodeGUID, choice.PortName);
            loadedChoices.Add(choiceButton);
        }
        
        if(choices.Count < 1)
        {
            var choiceButton = Instantiate(button, choicesBox.transform.position, choicesBox.transform.rotation, choicesBox.transform).GetComponent<DialogueChoice>();
            choiceButton.Initialize("END DIALOGUE", "Continue");
			loadedChoices.Add(choiceButton);
        }
        
        //Invoke("AdjustDialogueOptions", .05F);
        
    }

    private void ClearChoices()
    {
        if (loadedChoices.Count < 1)
            return;
        foreach (DialogueChoice choice in loadedChoices)
        {
            choice.DestroySelf(); //Destroy this
        }
        loadedChoices.Clear();
    }
    private void AdjustDialogueOptions()
    {
        float ypos = 0;
        int padding = 25;
        for(int i = 0; i < loadedChoices.Count; i++) 
        {
            loadedChoices[i].GetComponent<RectTransform>().anchoredPosition = new Vector2
                (loadedChoices[i].GetComponent<RectTransform>().anchoredPosition.x, ypos);
            ypos -= (padding + loadedChoices[i].GetComponent<RectTransform>().sizeDelta.y);
        }
    }
}
