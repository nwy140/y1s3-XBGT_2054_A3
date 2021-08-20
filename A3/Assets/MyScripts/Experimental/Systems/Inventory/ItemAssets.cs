using System.Collections;
using UnityEngine;


public class ItemAssets : Singleton<ItemAssets>
{
    private void Awake()
    {
    }
    public Transform pfItemWorld;

    public Sprite ItemSprite_WP_Melee_Sword;
    public Sprite ItemSprite_HP_Potion;
    public Sprite ItemSprite_MP_Potion;
    public Sprite ItemSprite_Currency_Coin;
    public Sprite ItemSprite_Medkit;


}