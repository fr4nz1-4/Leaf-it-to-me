using System;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public Sprite sprite;
    public CloseUpScript closeUpScript;
 
    private void OnMouseDown()
    {
        if (closeUpScript != null)
        {
            closeUpScript.ShowCloseUpPanel(sprite);
        }
        
        Sprite itemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        // Debug.Log(itemSprite.name);
        GameObject.Find("ItembarManager").GetComponent<ItembarScript>().add_item(itemSprite);
        gameObject.SetActive(false);
    }
}
