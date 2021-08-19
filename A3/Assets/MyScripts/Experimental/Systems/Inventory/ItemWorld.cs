using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity );
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }
    Item item;
    SpriteRenderer spriteRenderer;
    Light2D light2D;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        light2D = GetComponent<Light2D>();

    }

    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
        light2D.color = item.GetColor();
    }
}