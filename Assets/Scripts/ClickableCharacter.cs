using System;
using UnityEngine;

public class ClickableCharacter : MonoBehaviour
{
    // public MinigameScript miniGame;
    public DialogUIScript dialogScript;
    // public DialogManager dialogManager;
    // public DialogLine dialogLine;

    private void OnMouseDown()
    {
        Debug.Log("You clicked on: " + gameObject.name);

        // Dialog starten
        dialogScript.ShowDialogue(dialogScript.dialogLine);
    }
}