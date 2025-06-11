using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Random = UnityEngine.Random;

public class TicTacToeScript : MinigameScript
{
    public Image field_0_0;
    public Image field_0_1;
    public Image field_0_2;
    public Image field_1_0;
    public Image field_1_1;
    public Image field_1_2;
    public Image field_2_0;
    public Image field_2_1;
    public Image field_2_2;

    public Sprite circle;
    public Sprite cross;
    
    public Image connection_line_horizontal;
    public Image connection_line_diagonal;
    

    public void player_choice()
    {
        // player klick registrieren --> sprite darf nur einmal gesetzt werden!!
        // --> if field.sprite != empty
        // random reaktion des gegners --> Sprite setzen
    }
    // Methode die automatisch Sprite setzt und image auf nicht transparent setzen
    public void set_sprite(string move, Image field)
    {
        if (move == "circle")
        {
            field.sprite = circle;
        } else if (move == "cross")
        {
            field.sprite = cross;
        }
    }
}

