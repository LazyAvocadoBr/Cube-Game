using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;

public class KhaosPrefabManager : EditorWindow
{
    public VisualTreeAsset vAsset;   
    DropdownField dField;
    ScrollView sView;
    GameObject[] prefabs;
    string[] categories;
    int windowWidth;

    [MenuItem("Custom/SnapBlock")]
    public static void SnapBlocks()
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("GenericBlock"))
        {
            item.transform.position = new Vector3(Mathf.RoundToInt(item.transform.position.x),
                Mathf.RoundToInt(item.transform.position.y),
                Mathf.RoundToInt(item.transform.position.z));
        }
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("LaserBlocker"))
        {
            item.transform.position = new Vector3(Mathf.RoundToInt(item.transform.position.x),
                Mathf.RoundToInt(item.transform.position.y),
                Mathf.RoundToInt(item.transform.position.z));
        }
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Player"))
        {
            item.transform.position = new Vector3(Mathf.RoundToInt(item.transform.position.x),
                Mathf.RoundToInt(item.transform.position.y),
                Mathf.RoundToInt(item.transform.position.z));
        }
    }

   [MenuItem("Custom/PrefabManager")]
   public static void ShowWindow()
    {
        EditorWindow cWindow = GetWindow<KhaosPrefabManager>();
        cWindow.titleContent = new GUIContent("Prefab Manager");
    }
    public void CreateGUI()
    {
        rootVisualElement.Add(vAsset.Instantiate());       
        
        //Categories
        dField = rootVisualElement.Q<DropdownField>("Categories");
        dField.choices = new List<string>();
        categories = Directory.GetDirectories("Assets/Prefabs/");
        for(int i = 0; i < categories.Length; i++)
            dField.choices.Add(categories[i].Replace("Assets/Prefabs/",""));
        dField.index = 0;

        dField.RegisterValueChangedCallback(v => DrawWindow(dField.index));

        //refreshButton
        Button refreshButton = rootVisualElement.Q<Button>("refresh");
        refreshButton.clicked += () => DrawWindow(dField.index);

        //setup scrollview
        sView = rootVisualElement.Q<ScrollView>("Prefabs");
        sView.style.flexDirection = FlexDirection.Column;
        sView.RegisterCallback<GeometryChangedEvent>(v => DrawWindow(dField.index));
    }
    private void DrawWindow(int categoryIndex)
    {
        sView.Clear();
        //get assets to show
        string[] list = AssetDatabase.FindAssets("t:prefab", new[] { categories[categoryIndex] });
        prefabs = new GameObject[list.Length];

        for (int x = 0; x < prefabs.Length; x++)
            prefabs[x] = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(list[x]));
        
        Button prefabButton;
        int elementsPerRow = (int) sView.resolvedStyle.width/200;
        windowWidth = (int)sView.resolvedStyle.width;
        int numberOfElements = list.Length;
        int numberOfRows = numberOfElements / elementsPerRow;
        if (numberOfElements % elementsPerRow > 0)
            numberOfRows++;

        int prefabIndex = 0;
        for (int x = 0; x < numberOfRows; x++)
        {
            VisualElement vElement = new VisualElement();
            vElement.style.flexDirection = FlexDirection.Row;
            vElement.style.width = 300;

            for (int y = 0; y < elementsPerRow; y++)
            {
                if (prefabIndex < numberOfElements)
                {
                    prefabButton = new Button();
                    prefabButton.style.unityTextAlign = TextAnchor.LowerCenter;
                    prefabButton.text = prefabs[prefabIndex].transform.name;
                    prefabButton.style.width = 200;
                    prefabButton.style.height = 200;
                    prefabButton.style.backgroundImage = AssetPreview.GetAssetPreview(prefabs[prefabIndex]);
                    int index = prefabIndex;
                    prefabButton.clicked += () => LoadPrefab(index);
                    prefabIndex++;
                    vElement.Add(prefabButton);
                }
            }
            sView.Add(vElement);
        }
    }
    private void LoadPrefab(int index)
    {
        List<GameObject> newSelection = new List<GameObject>();
        GameObject go;
        
        if(Selection.activeGameObject != null)
        {
            foreach (GameObject item in Selection.gameObjects)
            {
                go = PrefabUtility.InstantiatePrefab(prefabs[index] as GameObject) as GameObject;
                Undo.RegisterCreatedObjectUndo(go, "Prefab replacement");
                go.transform.position = item.transform.position;
                go.transform.rotation = item.transform.rotation;
                go.transform.localScale = item.transform.localScale;
                DestroyImmediate(item);
                newSelection.Add(go);
            }
            Selection.objects = newSelection.ToArray();
        }
        else
        {
            go = PrefabUtility.InstantiatePrefab(prefabs[index] as GameObject) as GameObject;
            go.transform.position = SceneView.lastActiveSceneView.camera.transform.position + (SceneView.lastActiveSceneView.camera.transform.forward * 10);
            DestroyImmediate(Selection.activeGameObject);
            Selection.activeGameObject = go;
        }
    }
}
