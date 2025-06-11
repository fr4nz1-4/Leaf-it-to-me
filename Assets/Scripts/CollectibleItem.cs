using System;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    private void OnMouseDown()
    {
        Sprite itemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        // Debug.Log(itemSprite.name);
        GameObject.Find("ItembarManager").GetComponent<ItembarScript>().add_item(itemSprite);
        gameObject.SetActive(false);
    }
}
