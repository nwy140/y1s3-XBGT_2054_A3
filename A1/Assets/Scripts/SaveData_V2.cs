using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

[System.Serializable]
public class PlayerData
{
    public string Name;
    public int Atk;
    public int Def;
    public int HP;

}
[System.Serializable]
public class PlayerDataList
{
    public List<PlayerData> charaStats;
}


[ExecuteInEditMode]
public class SaveData_V2 : MonoBehaviour
{
   

    public PlayerDataList mdatas;
    public string filePath = "/Resources/saves/";
    public string filename_Json = "JsonSave";
    public string filename_XML = "XMLSave";
    public string keyName = "JsonSave123";

 
    public void SaveJSON()
    {
        string Json = JsonUtility.ToJson(mdatas); // convert/formatting to a single json string
        PlayerPrefs.SetString(keyName, Json);
        Debug.Log("saved data" + Json);
    }

    public void LoadJSON()
    {
       string result = PlayerPrefs.GetString(keyName);
       mdatas = JsonUtility.FromJson<PlayerDataList>(result);
       Debug.Log("Loaded data" + result);
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
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SaveTo_XML();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadFrom_XML();
        }
    }

}
