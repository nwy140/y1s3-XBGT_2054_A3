using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DontDestroyOnLoad : MonoBehaviour
{
    public bool isDestroyObjWithIdenticalName;
    public bool isDontDestroyOnLoadActivatedYet;

    private void Awake()
    {
        // Destroy Obj that has identical Name but not current Obj
        if (isDestroyObjWithIdenticalName == true)
        {
            List<DontDestroyOnLoad> IdenticalNameObj =
                new List<DontDestroyOnLoad>(GameObject.FindObjectsOfType<DontDestroyOnLoad>());
            foreach (DontDestroyOnLoad obj in IdenticalNameObj.ToArray())
            {
                if (obj.isDontDestroyOnLoadActivatedYet == false && obj != this)
                {
                    if (obj.name == gameObject.name)
                    {
                        Destroy(obj.gameObject);
                    }
                }
            }
        }
        
        if (transform.parent == null)
        {
            DontDestroyOnLoad(gameObject);
            isDontDestroyOnLoadActivatedYet = true;
        }
    }
}