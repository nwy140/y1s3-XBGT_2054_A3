using System.Collections;
using UnityEngine;

[System.Serializable]
public class Item
{
    public enum ItemType
    {
        Currency_Coin,
        HP_Potion,
        MP_Potion,
        WP_Melee_Sword,
        AOE_Use,
    }
    public ItemType itemType;
    public int amount;
    public ItemWorld itemWorldRef;
    public SupportCompSysInventory sysInventory;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Currency_Coin: return ItemAssets.Instance.ItemSprite_Currency_Coin;

            case ItemType.HP_Potion: return ItemAssets.Instance.ItemSprite_HP_Potion;
            case ItemType.MP_Potion: return ItemAssets.Instance.ItemSprite_MP_Potion;
            case ItemType.AOE_Use: return ItemAssets.Instance.ItemSprite_AOE_Use;
            case ItemType.WP_Melee_Sword: return ItemAssets.Instance.ItemSprite_WP_Melee_Sword;

        }
    }


    public Color GetColor() {
        switch (itemType)
        {
            default:
            case ItemType.Currency_Coin: return new Color(1, 1, 0);
            case ItemType.HP_Potion: return new Color(1, 0, 0);
            case ItemType.MP_Potion: return new Color(0, 0, 1);
            case ItemType.AOE_Use: return new Color(1, 0, 0);
            case ItemType.WP_Melee_Sword: return new Color(0,0,0);
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.WP_Melee_Sword:
            case ItemType.AOE_Use:
            case ItemType.HP_Potion:  
                return false;
            case ItemType.Currency_Coin:
            case ItemType.MP_Potion:  
                return true;
        }
    }
}