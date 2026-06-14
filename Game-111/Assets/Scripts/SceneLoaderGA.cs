using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SceneLoaderGA : GameActions
{
    [SerializeField]
    private int sceneIndex;

    public static Action<int> LoadScene = delegate { };
    public override void Action()
    {
        LoadScene(sceneIndex);
    }
    public void SetScene(int sIndex)
    {
        sceneIndex = sIndex;
    }
}
