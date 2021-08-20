using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }
    public Item item;
    SpriteRenderer spriteRenderer;
    Light2D light2D;
    public TextMeshProUGUI amountTextComp;

    #region Other Components
    public SupportCompSysInventory sysInventory;
    #endregion Other Components

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        light2D = GetComponent<Light2D>();
        SetItem(item);
    }

    //private void OnEnable()
    //{
    //    if (sysInventory != null)
    //    {
    //        transform.position = sysInventory.transform.position;
    //        sysInventory = null;
    //    }
    //}

    public void DropItem()
    {
        Vector2 randomDir = UnityEngine.Random.Range(0, 1) * Vector2.one + UnityEngine.Random.Range(0, 1) * Vector2.right;
        //ItemWorld itemWorld = SpawnItemWorld(dropPos, item);
        transform.position = Vector2.zero;
        transform.parent = null;
        sysInventory = null;
        gameObject.SetActive(true);
        GetComponent<Rigidbody2D>().AddForce(randomDir, ForceMode2D.Impulse);
    }

    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
        light2D.color = item.GetColor();

        amountTextComp.gameObject.SetActive(item.IsStackable());
        if (item.amount <= 1)
        {
            amountTextComp.text = "";
        }
        else
        {
            amountTextComp.text = item.amount.ToString();
        }
    }

    public Item GetItem()
    {
        return item;
    }
}