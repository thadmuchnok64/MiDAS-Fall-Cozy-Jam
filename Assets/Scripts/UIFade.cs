using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFade : MonoBehaviour
{
    public Material mat;
	public AnimationCurve curve;
	float timer = 0;
	float time = 1.5f;
	bool fadeIn = false;
	bool fadeOut = false;
	bool destroyed = false;


	private void OnEnable()
	{
		timer = 0;
		fadeIn = true;
		fadeOut = true;
		destroyed = true;
	}
	private void Update()
	{
		if (fadeIn && fadeOut)
		{
			timer += Time.deltaTime;
			mat.SetFloat("_Fade", curve.Evaluate(timer / time));
			if (timer > time)
				fadeIn = false;
		}
		else if (fadeOut && !fadeIn)
		{
			timer -= Time.deltaTime;
			mat.SetFloat("_Fade", curve.Evaluate(timer / time));
			if (timer < 0)
				fadeOut = false;
		}
		else if (!fadeOut && !fadeIn && destroyed)
		{
			HUDManager.Instance.GoToDefaultState();
			destroyed = false;

		}
	
	}
}
