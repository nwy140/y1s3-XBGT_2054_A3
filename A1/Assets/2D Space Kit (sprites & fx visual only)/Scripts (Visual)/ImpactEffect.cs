using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactEffect : MonoBehaviour
{
    public GameObject shoot_effect;
    public GameObject hit_effect;
    public GameObject instigator;

    public float lifespan = 5f;

    private void Awake()
    {
        if (isFilterIgnoreInstigatorTag)
        {
            onHitBoxDmgFilterIgnoreTags.Add(instigator.tag);
        }
    }
    // Use this for initialization
    void Start()
    {
        GameObject obj = Instantiate(shoot_effect, transform.position - new Vector3(0, 0, 5), Quaternion.identity); //Spawn muzzle flash
        obj.transform.parent = instigator.transform;
        //Destroy(gameObject, lifespan); //Bullet will despawn after 5 seconds

    }

    // Update is called once per frame
    void Update()
    {

    }



    #region SInt mods

    [Header("Physics Filtering")]
    public bool isFilterTag;
    public List<string> onHitBoxDmgFilterTags;
    public bool isFilterIgnoreInstigatorTag = true;
    public List<string> onHitBoxDmgFilterIgnoreTags;
    public List<GameObject> onHitBoxDmgFilterIgnoreObj;

    public List<int> onHitBoxDmgValue;

    #endregion

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
    void OnHitBoxDMG(GameObject other)
    {
        Debug.Log("HitBox Detected: " + other.name);
        if (other != instigator)
        {
            if ((onHitBoxDmgFilterTags.Contains(other.tag) == false && isFilterTag == false)
                && onHitBoxDmgFilterIgnoreTags.Contains(other.tag) == false
                && onHitBoxDmgFilterIgnoreObj.Contains(other) == false)
            {
                Instantiate(hit_effect, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                Destroy(gameObject, 0.1f);
            }
        }
    }

}
