using System;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public Sprite closeUpSprite;
    public CloseUpScript closeUpScript;

    private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void OnMouseDown()
    { 
        closeUpScript.ShowCloseUpPanel(closeUpSprite);
        // Debug.Log(itemSprite.name);
        GameObject.Find("ItembarManager").GetComponent<ItembarScript>().add_item(closeUpSprite);
        
        if (gameObject.name != "logs")
        {
            gameObject.SetActive(false);
        }
    }
    
    private void OnMouseOver()
    {
        if (InputBlocker.Instance.IsBlocked) return;

        Debug.Log("You hovered over: " + gameObject.name);
        ColorUtility.TryParseHtmlString("#BCBCBC", out Color color);
        _spriteRenderer.color = color;
    }
    
    private void OnMouseExit()
    {
        _spriteRenderer.color = Color.white;
    }
}
