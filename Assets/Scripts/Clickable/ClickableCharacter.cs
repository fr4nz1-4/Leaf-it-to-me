using System;
using UnityEngine;

public class ClickableCharacter : MonoBehaviour
{
    // public MinigameScript miniGame;
    public DialogUIScript dialogScript;
    // public DialogManager dialogManager;
    public DialogLine dialogLine;
    private SpriteRenderer _spriteRenderer;
    public LayerMask interactableLayer;
    private Vector3 _standardScale;
    private Vector3 _transformedScale;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _standardScale = gameObject.transform.localScale;
    }

    private void OnMouseDown()
    {
        if (InputBlocker.Instance.IsBlocked) return;

        Debug.Log("You clicked on: " + gameObject.name);

        if (gameObject.name == "tic-tac-toe_child")
        {
            dialogScript.ShowDialogue(dialogLine, false);   
        }
        else
        {
            // Dialog starten
            dialogScript.ShowDialogue(dialogLine, true);
        }
    }

    private void OnMouseOver()
    {
        if (InputBlocker.Instance.IsBlocked) return;

        Debug.Log("You hovered over: " + gameObject.name);
        ColorUtility.TryParseHtmlString("#BCBCBC", out Color color);
        _spriteRenderer.color = color;
        // gameObject.transform.localScale = standardScale * 1.02f;
    }
    
    private void OnMouseExit()
    {
        _spriteRenderer.color = Color.white;
        // gameObject.transform.localScale = standardScale;
    }
}