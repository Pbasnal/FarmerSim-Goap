using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RefreshLocalDotnetPlugins))]
public class LocalPluginRefreshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var myScript = (RefreshLocalDotnetPlugins)target;
        if (GUILayout.Button("Refresh Libraries"))
        {
            myScript.RefreshLibraries();
        }
    }
}
