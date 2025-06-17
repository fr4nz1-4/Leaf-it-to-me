using System;
using UnityEngine;

public class ClickableCharacter : MonoBehaviour
{
    // public MinigameScript miniGame;
    public DialogUIScript dialogScript;
    // public DialogManager dialogManager;
    public DialogLine dialogLine;
    private SpriteRenderer spriteRenderer;
    public LayerMask interactableLayer;
    private Vector3 standardScale;
    private Vector3 transformedScale;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (InputBlocker.Instance.IsBlocked) return;

        Debug.Log("You clicked on: " + gameObject.name);

        // Dialog starten
        dialogScript.ShowDialogue(dialogLine);
    }

    private void OnMouseOver()
    {
        if (InputBlocker.Instance.IsBlocked) return;

        Debug.Log("You hovered over: " + gameObject.name);
        ColorUtility.TryParseHtmlString("FFAE00", out Color color);
        spriteRenderer.color = color;
    }
    
    private void OnMouseExit()
    {
        spriteRenderer.color = Color.white;
    }
}