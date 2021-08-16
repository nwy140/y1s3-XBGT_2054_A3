using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.Xml.Serialization;
using System.IO;
[ExecuteInEditMode]
public class TileGenerator_CustomInspectora : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TileGenerator currentInstance = (TileGenerator)target;
        if (GUILayout.Button("Generate!!!"))
        {
            currentInstance.GenerateTile();
            //TileGenerator.GenerateTile();
        }

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
[System.Serializable]
public class TileTypes
{
    public GameObject TilePrefab;
    public int types;
}

public enum tileType
{
    sand ,
    dirt ,
    water ,
    rock ,
    shallowwater,
}
public class TileGenerator : MonoBehaviour
{
    public List<GameObject> TilePrefab;
    public int maxRow = 2;
    public int maxCol = 2;
    public float offsetX = 1f;
    public float offsetY = 1f;

    string mapName = "SampleMap";
    public void GenerateTile()
    {
        if (maxRow > 1000 || maxCol > 1000)
        {
            Debug.LogError("Generating too many tiles... May take a while..");
        }
        else
        {
            GameObject genTileParent = new GameObject();
            for (int i = 0; i < maxRow; i++)
            {
                for (int j = 0; j < maxRow; j++)
                {
                   var tile = Instantiate(TilePrefab[Random.Range(0,TilePrefab.Count)], Vector2.right * offsetX * i + Vector2.up * offsetY * j, Quaternion.identity);
                }
            }
        }
    }
    //private void Start()
    //{
    //    GenerateTile();
    //}

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveTo_XML();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadFrom_XML();
        }
    }

}
