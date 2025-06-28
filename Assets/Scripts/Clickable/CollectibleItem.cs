using System;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public Sprite closeUpSprite;
    public CloseUpScript closeUpScript;

    private SpriteRenderer spriteRenderer;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void OnMouseDown()
    { 
        closeUpScript.ShowCloseUpPanel(closeUpSprite);
        // Debug.Log(itemSprite.name);
        GameObject.Find("ItembarManager").GetComponent<ItembarScript>().add_item(closeUpSprite);
        gameObject.SetActive(false);
    }
    
    private void OnMouseOver()
    {
        if (InputBlocker.Instance.IsBlocked) return;

        Debug.Log("You hovered over: " + gameObject.name);
        ColorUtility.TryParseHtmlString("#BCBCBC", out Color color);
        spriteRenderer.color = color;
    }
    
    private void OnMouseExit()
    {
        spriteRenderer.color = Color.white;
    }
}
