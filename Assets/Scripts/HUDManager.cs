using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;
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
    }

    /// <summary>
    /// Toggles a dialogue animation for the canvas
    /// </summary>
    /// <param name="starting"> Is this starting (true) or ending (false) a dialogue?</param>
    public void ToggleDialogue(bool starting)
    {
        Debug.LogError("finish the fackin method ):<");
        // TODO: Add an animation for the HUD to have the dialogue system fade in
    }
}
