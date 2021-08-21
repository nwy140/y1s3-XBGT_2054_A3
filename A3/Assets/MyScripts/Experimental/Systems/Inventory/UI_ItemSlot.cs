using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour
{
    public Image itemIconIMG;
    public TextMeshProUGUI amountTextComp;
    public Transform ItemWorldParent;
    public UI_Inventory uiInventory;


    public void DropItem()
    {
        if (ItemWorldParent.childCount > 0)
        {
            var itemWorld = ItemWorldParent.GetChild(0).GetComponent<ItemWorld>();
            ItemWorldParent.DetachChildren();
            itemWorld.transform.parent = null;
            itemWorld.gameObject.SetActive(true);
            itemWorld.transform.position = itemWorld.sysInventory.transform.position.x * Vector2.right + itemWorld.sysInventory.transform.position.y * Vector2.up;
            itemWorld.sysInventory.inventory.RemoveItem(itemWorld.item);
            itemWorld.sysInventory = null;
            Destroy(gameObject);
        }
    }

    public void UseItem() // Wrapper
    {
        if (ItemWorldParent.childCount > 0)
        {
            var itemWorld = ItemWorldParent.GetChild(0).GetComponent<ItemWorld>();
            itemWorld.sysInventory.UseItem(itemWorld.item);
            AudioManager.instance.PlaySFX("use");
        }
    }
}