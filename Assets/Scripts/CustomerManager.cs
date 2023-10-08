using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class CustomerManager : MonoBehaviour
{
   public static CustomerManager Instance;
    public Customer currentCustomer;
   public Item heldItem;
    bool withinBounds = false;
    public UIFade fade;
    public List<Customer> listOfCustomers = new List<Customer>();
    public Customer everyone;
    bool readyForCustomer = false;
    bool over = false;
    bool actuallyOverForRealThisTime = false;
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
        listOfCustomers.ForEach(c => c.customerSatisfied = false);
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
        if (!over)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            if (withinBounds && Input.GetMouseButtonUp(0))
            {
                if (currentCustomer.correctItemName == heldItem.itemName)
                {
                    heldItem.DestroyItem();
                    DialogueManager.instance.RequestDialogue(currentCustomer.happyDialogueContainer, currentCustomer.icon);
                    currentCustomer.customerSatisfied = true;
                    Invoke("EndCustomer", .2f);
                }
                //JJ added, if no work delete
                else if (currentCustomer.correctItemName != heldItem.itemName)
                {
                    heldItem.DestroyItem();
                    DialogueManager.instance.RequestDialogue(currentCustomer.sadDialogueContainer, currentCustomer.icon);
                    currentCustomer.customerSatisfied = true;
                    Invoke("EndCustomer", .2f);
                }
                //end of jj
            }
            if (readyForCustomer && currentCustomer != null && currentCustomer.customerSatisfied && !HUDManager.Instance.isOccupied && HUDManager.Instance.ReadyForNewCustomer())
            {
                readyForCustomer = false;
                currentCustomer = null;
                Invoke("IterateCustomer", 3);

            }
            else if (currentCustomer == null && !HUDManager.Instance.isOccupied)
            {
                try
                {
                    currentCustomer = listOfCustomers.Where(c => c.customerSatisfied != true).FirstOrDefault();
                    if (currentCustomer == null)
                    {
						// end the game
						int i = 0;
						over = true;
						DialogueManager.instance.RequestDialogue(currentCustomer.dialogueContainer, currentCustomer.icon);
					}
                    if (readyForCustomer == false)
                        Invoke("CustomerWalkIn", Random.Range(10, 15));
                    readyForCustomer = true;
                }
                catch
                {
                    // end the game
                    int i = 0;
                    over = true;
                }
            }
        }
        else
        {
            if (!actuallyOverForRealThisTime)
            {
                Invoke("EndGame", 12);
                actuallyOverForRealThisTime= true;
            }
        }
        
	}

    public void EndGame()
    {
		DialogueManager.instance.RequestDialogue(everyone.dialogueContainer, everyone.icon);
        HUDManager.Instance.EndGame();
	}

	public void CustomerWalkIn()
    {
        if (currentCustomer == null)
            return; //end game
        SoundEffectRequest.instance.PlayDoorBell();
		DialogueManager.instance.RequestDialogue(currentCustomer.dialogueContainer, currentCustomer.icon);
	}
    public void IterateCustomer()
    {
		currentCustomer = null;
		HUDManager.Instance.Fade();
	}

    public void EndCustomer()
    {
        HUDManager.Instance.CharacterExit();
    }



}
