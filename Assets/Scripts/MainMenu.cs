using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Animator anim;
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void CreditsToggle()
    {
        anim.SetBool("Credits", !anim.GetBool("Credits"));
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
