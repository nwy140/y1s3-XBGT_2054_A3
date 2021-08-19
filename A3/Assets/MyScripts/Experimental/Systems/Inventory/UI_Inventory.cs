using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//ref: https://youtu.be/2WnAOV7nHW0
public class UI_Inventory : MonoBehaviour
{
    Inventory inventory;
    Transform itemSlotContainer;
    Transform itemSlotTemplate;
    private void Awake()
    {
        if(itemSlotContainer == null)
        {

        }
        if(itemSlotTemplate == null)
        {
            //itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        }
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        RefreshInventoryItems();
    }

    void RefreshInventoryItems()
    {
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 30f;
        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform =Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.GetChild(0).GetComponent<Image>();
            image.sprite = item.GetSprite();
            x++;
            if (x > 4)
            {
                x = 0;
                y++;
            }
        }
    }
}
