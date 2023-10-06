using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
   public static CustomerManager Instance;
    public Customer currentCustomer;
   public string heldItem;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("You have multiple customer manager scripts! This is a singleton so there should only be one!");
            Destroy(this);
        }
    }

    public void PortaitEvent()
    {
        if(currentCustomer.correctItemName.Equals(heldItem))
        {
            DialogueManager.instance.RequestDialogue(currentCustomer.happyDialogueContainer,currentCustomer.icon);
        }
    }


}
