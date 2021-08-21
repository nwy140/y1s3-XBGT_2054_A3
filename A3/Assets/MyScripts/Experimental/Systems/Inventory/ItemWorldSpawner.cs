using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//ref: https://youtu.be/2WnAOV7nHW0
// Redundant
public class ItemWorldSpawner : MonoBehaviour
{
    public Item item;
    public void SpawnRandomItem()
    {

        item = new Item { itemType = (Item.ItemType)UnityEngine.Random.Range(1,3), amount = 1 };
        ItemWorld.SpawnItemWorld(transform.position, item);
    }
}
