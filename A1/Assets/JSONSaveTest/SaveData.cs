using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


// https://docs.unity3d.com/ScriptReference/JsonUtility.FromJson.html

[System.Serializable]
public class CharData
{
    public string Name;
    public int Atk;
    public int Def;
    public int HP;
}

[System.Serializable]

public class CharDataList
{
    public List<CharData> charStats;
}

public class SaveData : MonoBehaviour
{
    public CharDataList mdata;
    public string keyName = "JsonSave123";
    private void Start()
    {
        //mdata.charStats = new List<CharData>(); // use this only for private lists data
    }
    public void SaveJSON()
    {
        string Json = JsonUtility.ToJson(mdata);
        PlayerPrefs.SetString(keyName, Json);
        Debug.Log("Saved data" + Json);
    }

    public void LoadJSON()
    {
        string result = PlayerPrefs.GetString(keyName);
        mdata = JsonUtility.FromJson<CharDataList>(result);
        Debug.Log("Loaded data" + result);
    }

    public void SaveJSON_FILE()
    {
        string Json = JsonUtility.ToJson(mdata);
        PlayerPrefs.SetString(keyName, Json);
        Debug.Log("Saved data" + Json);
    }
    public void LoadJSON_FILE()
    {
        string Json = JsonUtility.ToJson(mdata);
        PlayerPrefs.SetString(keyName, Json);
        Debug.Log("Saved data" + Json);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SaveJSON();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            LoadJSON();
        }
    }

}
