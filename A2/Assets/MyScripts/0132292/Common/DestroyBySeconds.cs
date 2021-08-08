using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBySeconds : MonoBehaviour
{
    public float destroyTimer = 3f;
    public bool isAllowDisable = true;
    private void Start()
    {
        if(isAllowDisable)
            Destroy(gameObject, destroyTimer);
    }
}
