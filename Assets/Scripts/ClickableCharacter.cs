using System;
using UnityEngine;

public class ClickableCharacter : MonoBehaviour
{
    public MinigameManager miniGameManager;
    public DialogManager dialogManager;
    
    private void OnMouseDown()
    {
        Debug.Log("You clicked on: " + gameObject.name); 
        
        switch (gameObject.name)
        {
            case "animal_fairy":
                Debug.Log("I'm the animal fairy. we will play rock paper scissors now");
                // miniGameManager.ShowMinigamePanel();     
                dialogManager.StartDialog();
                break;
        }
    }
}