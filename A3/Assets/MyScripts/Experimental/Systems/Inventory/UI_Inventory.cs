using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//ref: https://youtu.be/2WnAOV7nHW0
public class UI_Inventory : MonoBehaviour
{
    public Inventory inventory;
    public Transform itemSlotContainer;
    public Transform itemSlotTemplate;

    private void Awake()
    {
        if (itemSlotContainer == null)
        {

        }
        if (itemSlotTemplate == null)
        {
            //itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        }
    }

    public void UseItemByUIItemIndex(int index) // Wrapper
    {
        var uiItemSlots = GetComponentsInChildren<UI_ItemSlot>();

        if (uiItemSlots.Length > 0 && uiItemSlots.Length>index)
        {
            uiItemSlots[index].UseItem();
        }
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        RefreshInventoryItems();
    }

    void RefreshInventoryItems()
    {
        //foreach (Transform child in itemSlotContainer)
        //{
        //    Destroy(child.gameObject);
        //}

        int x = 0;
        int y = 0;
        float itemSlotCellSize = 30f;
        foreach (Item item in inventory.GetItemList())
        {


            bool isExistingItemFound = false;
            RectTransform itemSlotRectTransform = null;
            UI_ItemSlot ui_ItemSlot = null;
            foreach (Transform i in itemSlotContainer)
            {
                ui_ItemSlot = i.GetComponent<UI_ItemSlot>();
                if (ui_ItemSlot.itemIconIMG.sprite == item.GetSprite())
                {
                    isExistingItemFound = true;
                    itemSlotRectTransform = i.GetComponent<RectTransform>();
                    break;
                }
            }
            if (isExistingItemFound == false)
            {
                itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
                ui_ItemSlot = itemSlotRectTransform.GetComponent<UI_ItemSlot>();
                //itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
                Image itemIconImg = ui_ItemSlot.itemIconIMG;
                itemIconImg.sprite = item.GetSprite();
            }
            if (ui_ItemSlot.ItemWorldParent)
            {
                if (ui_ItemSlot.ItemWorldParent.childCount > 0)
                {
                   var itemWorld = ui_ItemSlot.ItemWorldParent.GetChild(0).GetComponent<ItemWorld>();
                    if (itemWorld.sysInventory)
                    {
                        if(itemWorld.sysInventory.inventory.GetItemList().Contains(itemWorld.item) == false)
                        {
                            Destroy(itemSlotRectTransform.gameObject);
                            break;
                        }

                    }
                }
            }

            itemSlotRectTransform.gameObject.SetActive(true);

            item.itemWorldRef.transform.parent = ui_ItemSlot.ItemWorldParent;
            TextMeshProUGUI amountTextComp = ui_ItemSlot.amountTextComp;

            if (amountTextComp != null)
            {
                amountTextComp.gameObject.SetActive(item.IsStackable());
                if (item.amount <= 1)
                {
                    amountTextComp.text = "";
                }
                else
                {
                    amountTextComp.text = item.amount.ToString();
                }
            }

            x++;
            if (x > 4)
            {
                x = 0;
                y++;
            }

        }
    }

}
