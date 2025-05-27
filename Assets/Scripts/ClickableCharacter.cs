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
        // dialogManager.startDialog(dialogLine);
        
        //würde dialogline am liebsten aus dialogscript nehmen
        dialogScript.ShowDialogue(dialogScript.dialogLine); // immer fehler, dass dialogline == null ist --> warum?
    }
}