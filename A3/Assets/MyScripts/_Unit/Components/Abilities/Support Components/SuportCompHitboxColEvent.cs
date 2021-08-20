using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuportCompHitboxColEvent : MonoBehaviour
{

    public UnitRefs _OwnerUnitRefs { get => _ownerUnitRefs; set => _ownerUnitRefs = value; }
    public UnitRefs _ownerUnitRefs;

    #region SInt mods

    public LayerMask allowedLayers; // layers that targeting system detects
    public LayerMask occlusionLayers; // layers that targeting system block Linecast
    //public float occlusionCastOffsetHeight;
    //public Color occlusionCastColor = Color.red;
    //public Color sensedCastColor = Color.green;
    public bool isUseAllowedOnlyTags;
    public List<string> allowedOnlyTags;
    public List<string> ignoreTags;
    public List<GameObject> ignoreObjs;

    #endregion SInt mods

    #region Other Components
    public SupportCompSysInventory sysInventory;
    #endregion Other Components

    #region Unity 2D Collision Events
    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ValidateHitboxDetectedTagsLayers(other.gameObject))
        {
            //if (sysInventory)
            //{
            //    ItemWorld itemWorld = null;
            //    bool hasItemWorld = other.TryGetComponent<ItemWorld>(out itemWorld);
            //    if (hasItemWorld)
            //    {
            //        sysInventory.inventory.AddItem(itemWorld.GetItem());
            //        itemWorld.sysInventory = sysInventory;
            //        //itemWorld.DestroySelf
            //        //Destroy(other.gameObject);
            //        other.transform.SetParent(sysInventory.transform);
            //        other.gameObject.SetActive(false);
            //    }
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
    }
    private void OnTriggerStay2D(Collider2D other)
    {
    }
    #endregion Unity 3D Collision Events
    #region Unity 3D Collision Events
    private void OnCollisionEnter(Collision collision)
    {
    }
    private void OnCollisionExit(Collision collision)
    {

    }
    private void OnCollisionStay(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }
    private void OnTriggerStay(Collider other)
    {

    }
    #endregion Unity 3D Collision Events

    public bool ValidateHitboxDetectedTagsLayers(GameObject other)
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

}
