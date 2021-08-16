using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveData))]
public class FileSaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SaveData currentSave = (SaveData)target;
        if(GUILayout.Button("Save Data Json"))
        {
            currentSave.SaveTo_Json();
        }

        if (GUILayout.Button("Load Data Json"))
        {
            currentSave.LoadFrom_Json();
        }

        if (GUILayout.Button("Save Data XML"))
        {
            currentSave.SaveTo_XML();
        }

        if (GUILayout.Button("Load Data XML"))
        {
            currentSave.LoadFrom_XML();
        }

       
    }
}
