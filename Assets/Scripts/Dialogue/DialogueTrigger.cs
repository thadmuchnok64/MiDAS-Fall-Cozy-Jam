using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger: MonoBehaviour
{
    public Customer customer;
    bool used = false;


	public void RequestDialogue()
    {
        DialogueManager.instance.RequestDialogue(customer.dialogueContainer,customer.icon);
        CustomerManager.Instance.currentCustomer = customer;
        used = true;
    }
}
