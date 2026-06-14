using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
public class SetScene : MonoBehaviour
{
#if UNITY_EDITOR
    public SceneAsset sceneAsset;
#endif
    public int sceneIndex;
    [SerializeField]
    private SceneLoaderGA loader;

    private void Start()
    {
        loader.SetScene(sceneIndex);
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(sceneAsset)))
        {     
            //Get Scenelist
            List<EditorBuildSettingsScene> activeSceneList = new List<EditorBuildSettingsScene>();
            for(int i = 0;i < EditorBuildSettings.scenes.Length;i++)
                activeSceneList.Add(EditorBuildSettings.scenes[i]);

            //Check if scene already exists in scene list
            foreach (EditorBuildSettingsScene item in activeSceneList)
            {
                if (item.path == AssetDatabase.GetAssetPath(sceneAsset))
                {
                    sceneIndex = GetSceneIndex(item.path);
                    return;
                }
            }

            //add new scene to sceneList
            activeSceneList.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetOrScenePath(sceneAsset),true));
            EditorBuildSettings.scenes = activeSceneList.ToArray();

            //update sceneIndex
            sceneIndex = GetSceneIndex(AssetDatabase.GetAssetPath(sceneAsset));
        }
    }
    private int GetSceneIndex(string path)
    {
        for(int i = 0; i < EditorBuildSettings.scenes.Length;i++)
        {
            if(path == EditorBuildSettings.scenes[i].path)
                return i;
        }
        return -1;
    }
#endif
}
