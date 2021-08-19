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
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
    }
}
