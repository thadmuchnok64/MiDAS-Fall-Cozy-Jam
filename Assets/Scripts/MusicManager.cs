using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource aud;
    public AudioClip loadedClip;
    public MusicManager instance;

    public void SetMusic()
    {
        aud.clip = loadedClip;
        aud.Play();
    }

    public void LoadClip()
    {
        loadedClip = CustomerManager.Instance.currentCustomer.music;
    }
}
