using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;
    Animator anim;
    public Image characterPortrait;
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

    /// <summary>
    /// Toggles a dialogue animation for the canvas
    /// </summary>
    /// <param name="starting"> Is this starting (true) or ending (false) a dialogue?</param>
    public void ToggleDialogue(bool starting,Sprite portrait = null)
    {
        if (portrait != null)
        {
            characterPortrait.sprite = portrait;

		}
        if(starting)
        {
            anim.SetBool("DialogueActive", true);
        }
        else
        {
			anim.SetBool("DialogueActive", false);
		}
	}
}
