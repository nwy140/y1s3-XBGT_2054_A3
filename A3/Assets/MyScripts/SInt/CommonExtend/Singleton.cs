using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ref: https://youtu.be/GopQFr6Q5IM?list=PLWYGofN_jX5BupV2xLjU1HUvujl_yDIN6&t=338
public class Singleton<T> : MonoBehaviour where T:MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            instance = (T)FindObjectOfType(typeof(T));
            if (instance == null) {
                GameObject obj = new GameObject();
                instance = obj.AddComponent<T>();
                obj.name = typeof(T).ToString();
            }
            return instance;
        }
    }
}
