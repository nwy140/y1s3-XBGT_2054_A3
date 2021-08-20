using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ref: https://youtu.be/2WnAOV7nHW0
public class Inventory
{
    List<Item> itemList;
    public List<Item> GetItemList()
    {
        return itemList;
    }
    public Inventory()
    {
        itemList = new List<Item>();

        AddItem(new Item { itemType = Item.ItemType.WP_Melee_Sword, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.HP_Potion, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.MP_Potion , amount = 1 });


    }
    public void AddItem(Item item)
    {
        itemList.Add(item);
    }
}
