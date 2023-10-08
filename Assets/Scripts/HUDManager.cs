using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;
    Animator anim;
    public Image characterPortrait;
    public bool isOccupied;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("There are multiple HUDManager scripts! There should only be one!!!");
        }
        anim = GetComponent<Animator>();
    }

    public bool ReadyForNewCustomer()
    {
		return !(anim.GetBool("DialogueActive") || anim.GetBool("CharacterActive"));


	}
	/// <summary>
	/// Toggles a dialogue animation for the canvas
	/// </summary>
	/// <param name="starting"> Is this starting (true) or ending (false) a dialogue?</param>
	public void ToggleDialogue(bool starting,Sprite portrait = null)
    {
        if (isOccupied && starting == true)
            return;
        isOccupied = true;
        if (portrait != null)
        {
            characterPortrait.sprite = portrait;

		}
        if(starting)
        {
            anim.SetBool("DialogueActive", true);
            anim.SetBool("CharacterActive", true);
        }
        else
        {
            isOccupied = false;
			anim.SetBool("DialogueActive", false);
		}
	}

    public void EndGame()
    {
        anim.SetBool("EndGame", true);
    }
    public void GoToDefaultState()
    {
        isOccupied = false;
        anim.Play("Default");
    }
    public void CharacterExit()
    {
        anim.SetBool("CharacterActive", false);
    }

    public bool Fade()
    {
        if (isOccupied)
            return false;
        isOccupied = true;
        anim.Play("Fade");
        return true;
    }
}
