using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ref: https://youtu.be/2WnAOV7nHW0
[System.Serializable]
public class Inventory
{
    public event EventHandler OnItemListChanged;
    public List<Item> itemList;

    Action<Item> useitemAction;
    public List<Item> GetItemList()
    {
        return itemList;
    }
    public Inventory(Action<Item> useItemAction)
    {
        this.useitemAction = useItemAction;
        itemList = new List<Item>();

        //AddItem(new Item { itemType = Item.ItemType.WP_Melee_Sword, amount = 1 });
        //AddItem(new Item { itemType = Item.ItemType.HP_Potion, amount = 1 });
        //AddItem(new Item { itemType = Item.ItemType.MP_Potion , amount = 1 });

    }

    public void RemoveItem(Item item)
    {
        {
            if (item.IsStackable())
            {
                Item itemInInventory = null;
                foreach (Item inventoryItem in itemList)
                {
                    if (inventoryItem.itemType == item.itemType)
                    {
                        inventoryItem.amount -= item.amount;
                        itemInInventory = inventoryItem;
                    }
                }
                if (itemInInventory != null && itemInInventory.amount <= 0)
                {
                    itemList.Remove(itemInInventory);
                }
            }
            else
            {
                itemList.Remove(item);
            }
 
            //itemList.Add(item);
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyinInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyinInventory = true;
                }
            }
            if(itemAlreadyinInventory == false)
            {
                itemList.Add(item);

            }
        }
        else
        {
            itemList.Add(item);
        }
        //itemList.Add(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }


    public void UseItem(Item item)
    {
        useitemAction(item);
    }
}
