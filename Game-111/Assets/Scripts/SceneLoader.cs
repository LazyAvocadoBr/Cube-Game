using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public bool bTest;
    [SerializeField]
    private int sceneIndex;

    public static Action FadeOut = delegate { };
    public static Action FadeIn = delegate { };
    public static Action DisableCameraDamp = delegate { };
    public static Action EnableCameraDamp = delegate { };
    public static Action UpdatePlayerTransform = delegate { };
    public static Action HideGoalScreen = delegate { }; 
    private bool bFadeOutComplete,bLoading;   
   
    private void OnEnable()
    {
        SceneLoaderGA.LoadScene += LoadScene;
        UIManager.LoadScene += LoadScene;
        FadeScreen.ScreenFadeOutComplete += FadeOutComplete;
    }
    private void OnDisable()
    {
        SceneLoaderGA.LoadScene -= LoadScene;
        UIManager.LoadScene -= LoadScene;
        FadeScreen.ScreenFadeOutComplete -= FadeOutComplete;
    }
    private void LoadScene(int value)
    {
        sceneIndex = value;
        if (bLoading) return;
        StartCoroutine(nameof(SceneLoadingSequence));
    }
    private void FadeOutComplete() 
    { 
        bFadeOutComplete = true;
    }
    IEnumerator SceneLoadingSequence()
    {
        bLoading = true;
        HideGoalScreen();
        FadeOut();
        while (!bFadeOutComplete)
            yield return new WaitForEndOfFrame();
        AsyncOperation loader = SceneManager.LoadSceneAsync(sceneIndex);

        while(loader.progress != 1)
            yield return new WaitForEndOfFrame();

        //updates player for camera tracking
        DisableCameraDamp();
        UpdatePlayerTransform();
        yield return new WaitForSeconds(0.1f);
        EnableCameraDamp();

        FadeIn();
        bLoading = false;
        bFadeOutComplete = false;
    }
}
