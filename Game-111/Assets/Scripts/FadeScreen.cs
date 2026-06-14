using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class FadeScreen : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Image fadeScreenImage;
    public static Action ScreenFadeInComplete = delegate { };
    public static Action ScreenFadeOutComplete = delegate { };   

    private float rate;
    private bool bActive, bFadeIn;
    private void OnEnable()
    {
        SceneLoader.FadeIn += FadeIn;
        SceneLoader.FadeOut += FadeOut;
    }
    private void OnDisable()
    {
        SceneLoader.FadeIn -= FadeIn;
        SceneLoader.FadeOut -= FadeOut;
    }
    private void FadeIn()
    {
        bFadeIn = true;
        if(bActive)
        {
            rate = 1 - rate;
        }
        else
        {
            StartCoroutine(nameof(ScreenFader));
        }
    }
    private void FadeOut()
    {
        bFadeIn = false;
        if(bActive) 
        {
            rate = 1 - rate;
        }
        else
        {
            StartCoroutine(nameof(ScreenFader));
        }
    }
    IEnumerator ScreenFader()
    {
        rate = 0;
        bActive = true;
        fadeScreenImage.enabled = true;
        canvas.enabled = true;  
        while (rate < 1f)
        {
            yield return new WaitForEndOfFrame();
            rate += Time.deltaTime * 2;
            if (bFadeIn)
            {
                fadeScreenImage.color = Color.Lerp(Color.black, Color.clear, rate);
            }
            else
            {
                fadeScreenImage.color = Color.Lerp(Color.clear, Color.black, rate);
            }
        }
        if (fadeScreenImage.color == Color.clear)
        {
            canvas.enabled = false;
            fadeScreenImage.enabled = false;
            ScreenFadeInComplete();
        }
        else
            ScreenFadeOutComplete();
        bActive = false;
    }
}
