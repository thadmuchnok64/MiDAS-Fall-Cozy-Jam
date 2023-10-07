using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
   public static CustomerManager Instance;
    public Customer currentCustomer;
   public string heldItem;
    bool withinBounds = false;
    public UIFade fade;
    public List<Customer> listOfCustomers = new List<Customer>();
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

    


    // I know this isn't the prettiest, but I had to work around a unity bug
    public void Enter()
    {
        withinBounds = true;
    }

	public void Leave()
	{
        withinBounds = false;

	}

	private void Update()
	{
        if (withinBounds && Input.GetMouseButtonUp(0))
        {
            if(currentCustomer.correctItemName == heldItem)
            {
                DialogueManager.instance.RequestDialogue(currentCustomer.happyDialogueContainer,currentCustomer.icon);
                currentCustomer.customerSatisfied = true;
                Invoke("EndCustomer", .2f);
			}
        }
        if(currentCustomer !=null && currentCustomer.customerSatisfied && !HUDManager.Instance.isOccupied && HUDManager.Instance.ReadyForNewCustomer())
        {
			currentCustomer = null;
            Invoke("IterateCustomer", 3);

		}
	}

    public void IterateCustomer()
    {
		HUDManager.Instance.Fade();
	}

    public void EndCustomer()
    {
        HUDManager.Instance.CharacterExit();
    }



}
