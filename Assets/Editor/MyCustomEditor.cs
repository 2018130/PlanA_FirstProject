using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MyCustomEditor : EditorWindow
{
    public static float staticFishSpeed = 1f;

    
    [MenuItem("Tools/My Custom Editor")]
    public static void ShowMyEditor()
    {
        EditorWindow window = GetWindow<MyCustomEditor>();
        window.titleContent = new GUIContent("My Custom Editor");
        window.maxSize = new Vector2(1000f, 1000f);
    }
    public void CreateGUI()
    {
        
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Fish");

        const float minSpeed = 0f, maxSpeed = 5f;
        staticFishSpeed = EditorGUILayout.Slider("Fish Speed", staticFishSpeed, minSpeed, maxSpeed);

        foreach(GameObject obj in Selection.gameObjects)
        {
            NewFishMove newFishMove = obj.GetComponent<NewFishMove>();
            if (newFishMove)
            {
                obj.GetComponent<NewFishMove>().SetFishSpeed(staticFishSpeed);
            }
        }
    }
}