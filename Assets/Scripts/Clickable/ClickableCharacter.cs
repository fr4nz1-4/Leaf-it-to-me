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
        standardScale = gameObject.transform.localScale;
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
        ColorUtility.TryParseHtmlString("#BCBCBC", out Color color);
        spriteRenderer.color = color;
        gameObject.transform.localScale = standardScale * 1.02f;
    }
    
    private void OnMouseExit()
    {
        spriteRenderer.color = Color.white;
        gameObject.transform.localScale = standardScale;
    }
}