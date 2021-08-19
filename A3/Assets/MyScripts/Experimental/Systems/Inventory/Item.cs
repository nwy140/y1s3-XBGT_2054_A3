using System.Collections;
using UnityEngine;


public class Item
{
    public enum ItemType
    {
        WP_Melee_Sword,
        HP_Potion,
        MP_Potion,
        Currency_Coin,
    }
    public ItemType itemType;
    public int amount;
}