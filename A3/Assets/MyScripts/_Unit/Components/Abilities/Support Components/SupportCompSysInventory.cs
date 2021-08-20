using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportCompSysInventory : MonoBehaviour, ISupportComp
{
    public UnitRefs _OwnerUnitRefs { get => _ownerUnitRefs; set => _ownerUnitRefs = value; }
    public UnitRefs _ownerUnitRefs;

    public Inventory inventory;
    [SerializeField]
    UI_Inventory uiInventory;

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
    void UseItem(Item item)
    {
        switch (item.itemType)
        {
            default:
            case Item.ItemType.WP_Melee_Sword:
            case Item.ItemType.MedKit:
            case Item.ItemType.Currency_Coin:
            case Item.ItemType.HP_Potion:
            case Item.ItemType.MP_Potion:
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
