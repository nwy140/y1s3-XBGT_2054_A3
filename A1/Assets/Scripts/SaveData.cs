using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
[System.Serializable]
public class CharacterData
{
    public string Name;
    public int HP;
    public int ATK;
    public int DEF;
    public List<string> itemName;
}

[ExecuteInEditMode]
public class SaveData : MonoBehaviour
{

    //steps
    //1.) Serialize to format you want to json/xml
    //2.) Store that serialize string to either playerpref or write into a txt document

    public string PlayerPrefKey = "Chara";
    public CharacterData myCharacter;
    //public List<CharacterData> charas;
    public string filePath = "/Resources/saves/";
    public string filename_Json = "JsonSave";
    public string filename_XML = "XMLSave";

    public bool PlayerPrefMode = true;
    public void SaveTo_Json()
    {
        string Json = JsonUtility.ToJson(myCharacter);
        Debug.Log(Json);
        if(PlayerPrefMode)
        {
            //save to playerpref
            PlayerPrefs.SetString(PlayerPrefKey, Json);
        }
       else
        {
            // save to textfile
            string dir = Application.dataPath + filePath + filename_Json + ".json";
            File.WriteAllText(dir, Json); // this does not instantly generated in editor, need to tab out
        }
       
    }

    public void LoadFrom_Json()
    {
        if (PlayerPrefMode)
        {
            //load from playerpref
            string result = PlayerPrefs.GetString(PlayerPrefKey);
            Debug.Log(result);
            myCharacter = JsonUtility.FromJson<CharacterData>(result);
        }
        else
        {
            string dir = Application.dataPath + filePath + filename_Json + ".json";
            if (File.Exists(dir))
            {
                string jsonReceiving = File.ReadAllText(dir);
                myCharacter = JsonUtility.FromJson<CharacterData>(jsonReceiving);
               
            }
     
        }
        
    }


    public void SaveTo_XML()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterData));
        FileStream stream = 
            new FileStream(Application.dataPath + filePath + filename_XML + ".xml", FileMode.Create);//similar to write all text
        serializer.Serialize(stream, myCharacter);
        stream.Close();

    }

    public void LoadFrom_XML()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterData));
        FileStream stream = new FileStream(Application.dataPath + filePath + filename_XML + ".xml", FileMode.Open);
        myCharacter = (CharacterData)serializer.Deserialize(stream);
        stream.Close();
    }
}
