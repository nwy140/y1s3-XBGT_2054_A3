using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideBomber : MonoBehaviour
{
    UnitRefs _ur;
    bool hasUR;


    public float aimRotOffset = -90;

    private void Awake()
    {
        hasUR = TryGetComponent(out _ur);
    }
    void Update()
    {
        var p1 = GameObject.FindGameObjectWithTag("Player").transform;
        var rot = Quaternion.Euler((Vector2Common.GetRotBetween2Pos(p1.position, transform.position) + aimRotOffset) * Vector3.forward);
        transform.rotation = rot;
    }

}
