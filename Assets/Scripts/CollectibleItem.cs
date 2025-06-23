using System;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public Sprite closeUpSprite;
    public CloseUpScript closeUpScript;

    private void OnMouseDown()
    { 
        closeUpScript.ShowCloseUpPanel(closeUpSprite);
        // Debug.Log(itemSprite.name);
        GameObject.Find("ItembarManager").GetComponent<ItembarScript>().add_item(closeUpSprite);
        gameObject.SetActive(false);
    }
}
