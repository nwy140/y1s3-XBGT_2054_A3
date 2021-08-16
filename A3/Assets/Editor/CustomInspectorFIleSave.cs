using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(SaveData_V2))]
public class CustomInspectorFIleSave : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SaveData_V2 currentInstance = (SaveData_V2)target;

        if (GUILayout.Button("SaveJSon"))
        {
            currentInstance.SaveJSON_FILE();
        }

        if (GUILayout.Button("LoadJSon"))
        {
            currentInstance.LoadJSON_FILE();
        }

        if (GUILayout.Button("SaveXML"))
        {
            currentInstance.SaveTo_XML();
        }

        if (GUILayout.Button("LoadXML"))
        {
            currentInstance.LoadFrom_XML();
        }
    }
}
