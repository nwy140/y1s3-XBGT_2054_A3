using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transform2DAutoLookAtObj2D : MonoBehaviour
{
    UnitRefs _ur;
    bool hasUR;

    Transform targetObj;
    bool targetIsPlayer = true;

    public float aimRotOffset = -90;

    private void Awake()
    {
        hasUR = TryGetComponent(out _ur);
    }
    void Update()
    {
        if (targetIsPlayer)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {

                Transform obj = GameObject.FindGameObjectWithTag("Player").transform;
                targetObj = obj;
            }
        }
        var rot = Quaternion.Euler((Vector2Common.GetRotBetween2Pos(targetObj.position, transform.position) + aimRotOffset) * Vector3.forward);
        transform.rotation = rot;
    }

}
