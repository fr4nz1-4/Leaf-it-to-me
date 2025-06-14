using System;
using UnityEngine;

public class ClickableCharacter : MonoBehaviour
{
    // public MinigameScript miniGame;
    public DialogUIScript dialogScript;
    // public DialogManager dialogManager;
    public DialogLine dialogLine;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        Debug.Log("You clicked on: " + gameObject.name);

        // Dialog starten
        dialogScript.ShowDialogue(dialogLine);
    }

    private void OnMouseOver()
    {
        Debug.Log("You hovered over: " + gameObject.name);
        ColorUtility.TryParseHtmlString("FFAE00", out Color color);
        spriteRenderer.color = color;
    }
    
    private void OnMouseExit()
    {
        spriteRenderer.color = Color.white;
    }
}