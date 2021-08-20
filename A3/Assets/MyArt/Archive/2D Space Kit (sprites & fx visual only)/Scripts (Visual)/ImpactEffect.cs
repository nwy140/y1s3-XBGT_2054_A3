using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImpactEffect : MonoBehaviour
{
    public GameObject shoot_effect;
    public GameObject hit_effect;
    public GameObject instigator;

    public float lifespan = 5f;
    public bool isAllowDestroy = true;

    private void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {
        GameObject obj = Instantiate(shoot_effect, transform.position - new Vector3(0, 0, 5), Quaternion.identity); //Spawn muzzle flash

        if (instigator)
        {
            obj.transform.parent = instigator.transform;
        }
        //Destroy(gameObject, lifespan); //Bullet will despawn after 5 seconds
        if (isIgnoreInstigatorTag && instigator)
        {
            ignoreTags.Add(instigator.tag);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }




    private void OnCollisionEnter(Collision other)
    {
        //Collision Damage
        OnHitBoxDMG(other.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnHitBoxDMG(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHitBoxDMG(other.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnHitBoxDMG(collision.gameObject);
    }
    #region Hitbox Filtering
    public LayerMask allowedLayers; // layers that could be detected
    //public LayerMask occlusionLayers; // layers that targeting system block Linecast

    public bool isUseAllowedOnlyTags;
    public bool isIgnoreInstigatorTag = true;
    public List<string> allowedOnlyTags;
    public List<string> ignoreTags;
    public List<GameObject> ignoreObjs;
    public bool ValidateTargetDetectedTagsLayers(GameObject other)
    {
        // Evaluate Tags and Layers
        if (isUseAllowedOnlyTags)
        {
            if (allowedOnlyTags.Contains(other.tag) == false)
            {
                return false; // target rejected
            }
        }
        if (ignoreTags.Contains(other.tag) /*|| ignoreLayers == (ignoreLayers | (1 << other.layer))*/)
        {
            return false;
        }
        if (ignoreObjs.Contains(other))
        {
            return false;
        }
        if (allowedLayers != (allowedLayers | (1 << other.layer)))
        {
            return false;
        }
        return true; // target accepted
    }
    #endregion Hitbox Filtering

    void OnHitBoxDMG(GameObject other)
    {
        //Debug.Log("HitBox Detected: " + other.name);
        if (ValidateTargetDetectedTagsLayers(other.gameObject) == true)
        {
            Instantiate(hit_effect, transform.position, Quaternion.identity);

            if (isAllowDestroy)
            {
                gameObject.SetActive(false);
                Destroy(gameObject, 0.1f);
            }
        }
    }


}
