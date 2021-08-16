using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjSpawnHelper : MonoBehaviour
{

    public List<GameObject> objsToSpawn;
    public bool isUseThisObjPos = true;
    public Vector3 spawnPos;
    public bool isUseThisObjRotEuler = true;
    public Vector3 spawnRotEuler;

    public List<GameObject> spawnedObjs;
    public int spawnedObjsMaxCount = 3;
    public void Update()
    {
        if (isUseThisObjPos)
        {
            spawnPos = transform.position;
        }
        if (isUseThisObjRotEuler)
        {
            spawnRotEuler = transform.eulerAngles;
        }
    }

    // Spawn OBJ From List
    public void SpawnAllObjs()
    {
        foreach (var obj in objsToSpawn)
        {
            Instantiate(obj, spawnPos, Quaternion.Euler(spawnRotEuler));
        }
    }

    public void SpawnRandomObj()
    {
        if(spawnedObjs.Count < spawnedObjsMaxCount)
        {
            var obj = objsToSpawn[Random.Range(0,objsToSpawn.Count)];
            var spawnedObj = Instantiate(obj, spawnPos, Quaternion.Euler(spawnRotEuler));
            spawnedObjs.Add(spawnedObj);
        }

        // remove null or destroyed objs
        for (var i = spawnedObjs.Count - 1; i > -1; i--)
        {
            if (spawnedObjs[i] == null)
                spawnedObjs.RemoveAt(i);
        }
    }
}
