#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class EnemyCreatorWindow : EditorWindow
{
    private string fileName = "EnemySO";
                
    [MenuItem("Tools/Enemy Creator")]

    public static void ShowWindow()
    {
        GetWindow<EnemyCreatorWindow>("Enemy Creator");
    }
    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUILayout.Label("Create New Enemy SO ", EditorStyles.boldLabel);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Create EnemySO", GUILayout.Height(30)))
        {
            CreateEnemyPreset();
        }
    }

    private void CreateEnemyPreset()
    {
        EnemySO enemy = ScriptableObject.CreateInstance<EnemySO>();


        string path = $"Assets/so/ENEMIES/{fileName}.asset";

        path = AssetDatabase.GenerateUniqueAssetPath(path);

        AssetDatabase.CreateAsset(enemy, path);

        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();

        Debug.Log("EnemyCreated");
    }
}
#endif
