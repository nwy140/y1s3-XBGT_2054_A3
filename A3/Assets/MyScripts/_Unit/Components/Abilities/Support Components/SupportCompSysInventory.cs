using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportCompSysInventory : MonoBehaviour, ISupportComp
{
    public UnitRefs _OwnerUnitRefs { get => _ownerUnitRefs; set => _ownerUnitRefs = value; }
    public UnitRefs _ownerUnitRefs;

    public Inventory inventory;
    [SerializeField]
    public UI_Inventory uiInventory;

    private void Awake()
    {
        inventory = new Inventory(UseItem);
        uiInventory.SetInventory(inventory);

        //ItemWorld.SpawnItemWorld(new Vector2(-8,5), new Item { itemType = Item.ItemType.HP_Potion, amount = 1 });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GetComponentInChildren<ItemWorld>().DropItem();
        }
    }
    public void UseItem(Item item)
    {
        switch (item.itemType)
        {
            default:
            case Item.ItemType.Currency_Coin:
                Debug.Log("Use " + "Sword");
                _ownerUnitRefs.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.ItemType0).buttonDown = true;
                break;
            case Item.ItemType.HP_Potion:
                _ownerUnitRefs.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.ItemType1).buttonDown = true;
                inventory.RemoveItem(item);
                
                break;
            case Item.ItemType.MP_Potion:
                _ownerUnitRefs.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.ItemType2).buttonDown = true;
                break;
            case Item.ItemType.AOE_Use:
                _ownerUnitRefs.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.ItemType3).buttonDown = true;
                break;
            case Item.ItemType.WP_Melee_Sword:
                _ownerUnitRefs.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.ItemType4).buttonDown = true;
                break;
                break;
        }
    }


    public void IntactPickupItem(GameObject other)
    {
        ItemWorld itemWorld = null;
        bool hasItemWorld = other.TryGetComponent<ItemWorld>(out itemWorld);
        if (hasItemWorld)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.sysInventory = this;
            //itemWorld.DestroySelf
            //Destroy(other.gameObject);
            other.transform.SetParent(transform);
            other.gameObject.SetActive(false);
        }
    }

}
