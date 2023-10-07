using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFade : MonoBehaviour
{
    public Material mat;
	public AnimationCurve curve;

	private void OnEnable()
	{
        StartCoroutine(FadeCor());
    }
    IEnumerator FadeCor()
    {
        for( float i = 0; i < 60; i++)
        {
            mat.SetFloat("_Fade", curve.Evaluate(i / 60.0f));
            yield return new WaitForEndOfFrame();
        }
		yield return new WaitForSeconds(.5f);
		for (float i = 60; i >= 0; i--)
		{
			mat.SetFloat("_Fade", curve.Evaluate(i / 60.0f));
			yield return new WaitForEndOfFrame();
		}
		HUDManager.Instance.GoToDefaultState();
	}
}
