using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource aud;
    public AudioClip loadedClip;
    public static MusicManager instance;

	private void Start()
	{
		if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
	}

	public void SetMusic(AudioClip clip)
    {
        StartCoroutine(fadeinclip(clip));
    }

    public IEnumerator fadeinclip(AudioClip clip)
    {
        for(float i= 0; i< 120; i++)
        {
            aud.volume = Mathf.Lerp(1, 0, i / 120f);
            yield return new WaitForEndOfFrame();
        }
        aud.Stop();
        aud.PlayOneShot(clip);
		for (float i = 0; i < 120; i++)
		{
			aud.volume = Mathf.Lerp(0, 1, i / 120f);
			yield return new WaitForEndOfFrame();

		}
	}
}
