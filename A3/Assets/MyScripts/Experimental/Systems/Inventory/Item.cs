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


    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.WP_Melee_Sword: return ItemAssets.Instance.ItemSprite_WP_Melee_Sword;
            case ItemType.HP_Potion: return ItemAssets.Instance.ItemSprite_HP_Potion;
            case ItemType.MP_Potion: return ItemAssets.Instance.ItemSprite_MP_Potion;
            case ItemType.Currency_Coin: return ItemAssets.Instance.ItemSprite_Currency_Coin;
        }
    }

    public Color GetColor() {
        switch (itemType)
        {
            default:
            case ItemType.WP_Melee_Sword: return new Color(0,0,0);
            case ItemType.HP_Potion: return new Color(1, 0, 0);
            case ItemType.MP_Potion: return new Color(0, 0, 1);
            case ItemType.Currency_Coin: return new Color(1, 1, 0);
        }
    }
}