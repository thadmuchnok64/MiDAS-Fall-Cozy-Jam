using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectRequest : MonoBehaviour
{
    public static SoundEffectRequest instance;
	public AudioSource aud;
	public AudioClip doorbell;
	private void Start()
	{
		if(instance == null)
		{
			instance = this;
		}
		else
		{
			Debug.Log("Multiple sound effect scripts!");
			Destroy(this);
		}

		aud = gameObject.GetComponent<AudioSource>();
	}

	public void PlayDoorBell()
	{
		aud.PlayOneShot(doorbell);
	}

	public void PlaySound(AudioClip clip)
	{
		aud.PlayOneShot(clip);
	}
}
