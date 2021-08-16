using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml.Serialization;

public class CustomEditorWindow : EditorWindow
{
    public PlayerDataList mdatas;
    public string filePath = "/Resources/saves/";
    public string filename_Json = "JsonSave";
    public string filename_XML = "XMLSave";
    public string keyName = "JsonSave123";

    [MenuItem("CUSTOMSAVE/My Custom save")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CustomEditorWindow));
    }

    public void SaveJSON_FILE()
    {
        string Json = JsonUtility.ToJson(mdatas); // convert/formatting to a single json string
        string dir = Application.dataPath + filePath + filename_Json + ".json";
        File.WriteAllText(dir, Json); // this does not instantly generated in editor, need to tab out
        Debug.Log("saved data" + Json);
    }

    public void LoadJSON_FILE()
    {
        string dir = Application.dataPath + filePath + filename_Json + ".json";
        if (File.Exists(dir))
        {
            string jsonReceiving = File.ReadAllText(dir);
            mdatas = JsonUtility.FromJson<PlayerDataList>(jsonReceiving);
            Debug.Log("Loaded data" + jsonReceiving);
        }
    }

    public void SaveTo_XML()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerDataList));
        FileStream stream =
            new FileStream(Application.dataPath + filePath + filename_XML + ".xml", FileMode.Create);//similar to write all text
        serializer.Serialize(stream, mdatas);
        stream.Close();

    }

    public void LoadFrom_XML()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerDataList));
        FileStream stream = new FileStream(Application.dataPath + filePath + filename_XML + ".xml", FileMode.Open);
        mdatas = (PlayerDataList)serializer.Deserialize(stream);
        stream.Close();
    }
    void OnGUI()
    {
       
        if (GUILayout.Button("SaveJSon"))
        {
            SaveJSON_FILE();
        }

        if (GUILayout.Button("LoadJSon"))
        {
            LoadJSON_FILE();
        }

        if (GUILayout.Button("SaveXML"))
        {
            SaveTo_XML();
        }

        if (GUILayout.Button("LoadXML"))
        {
            LoadFrom_XML();
        }
    }
}
