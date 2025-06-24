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
        GameObject.Find("Itembar_folded-out").GetComponent<ItembarScript>().add_item(closeUpSprite);
        gameObject.SetActive(false);
    }
}
