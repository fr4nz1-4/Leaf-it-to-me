using System;
using UnityEngine;

public class ClickableCharacter : MonoBehaviour
{
    public RockPaperScissorsScript RPSScript;
    public DialogManager dialogManager;
    
    private void OnMouseDown()
    {
        Debug.Log("You clicked on: " + gameObject.name); 
    }
}