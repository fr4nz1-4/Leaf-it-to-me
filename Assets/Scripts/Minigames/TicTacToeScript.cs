using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TicTacToeScript : MinigameScript
{
    public Image[] fields;
    
    public Sprite circle;
    public Sprite cross;
    
    public Image connectionLineHorizontal;
    public Image connectionLineDiagonal;
    
    private bool isCircleTurn = true;  // true = Spieler Kreis, false = KI Kreuz

    private void Start()
    {
        foreach (var field in fields)
        {
            field.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            var capturedField = field;
            var button = capturedField.GetComponent<Button>();

            button.onClick.AddListener(() =>
            {
                Debug.Log("Button clicked");
                HandleFieldClick(capturedField);
            });
        }
    }

    private void HandleFieldClick(Image field)
    {
        // Falls schon belegt -> ignorieren
        if (field.sprite != null)
            return;
        Debug.Log("enter handlefieldcheck method");

        // Sprite setzen
        if (isCircleTurn)
            SetSprite("circle", field);
        else
            SetSprite("cross", field);

        // Spieler wechseln
        isCircleTurn = !isCircleTurn;
    }
    
        // --> if field.sprite != empty
        // random reaktion des gegners --> Sprite setzen
    
    // Methode die automatisch Sprite setzt und image auf nicht transparent setzen
    private void SetSprite(string move, Image field)
    {
        if (move == "circle")
        {
            Debug.Log("set sprite circle");
            field.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            field.sprite = circle;
            
        }
        else if (move == "cross")
        {
            field.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            field.sprite = cross;
        }

        // Button danach deaktivieren
        field.GetComponent<Button>().interactable = false;
    }
}

