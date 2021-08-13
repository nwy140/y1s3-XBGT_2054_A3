using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Ref: https://youtu.be/BQGTdRhGmE4?list=PLuLJclBWmeWU2QpzE51U4Nz2FkG69Ijba
public class TransformPosShake : MonoBehaviour
{
    public bool start = false;
    public AnimationCurve curve;
    public float duration = 1f;
     
     void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking()
    {
        Vector3 startPos = transform.position;
        float elpasedTime = 0f;
        while (elpasedTime < duration)
        {
            elpasedTime += Time.deltaTime;
            float strength = curve.Evaluate(elpasedTime / duration);
            transform.position = startPos + Random.insideUnitSphere * strength  ;
            yield return null;
        }
    }
}
