using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TicTacToeScript : MinigameScript
{
    [FormerlySerializedAs("field_0_0")] public Image field00;
    [FormerlySerializedAs("field_0_1")] public Image field01;
    [FormerlySerializedAs("field_0_2")] public Image field02;
    [FormerlySerializedAs("field_1_0")] public Image field10;
    [FormerlySerializedAs("field_1_1")] public Image field11;
    [FormerlySerializedAs("field_1_2")] public Image field12;
    [FormerlySerializedAs("field_2_0")] public Image field20;
    [FormerlySerializedAs("field_2_1")] public Image field21;
    [FormerlySerializedAs("field_2_2")] public Image field22;

    public Sprite circle;
    public Sprite cross;
    
    [FormerlySerializedAs("connection_line_horizontal")] public Image connectionLineHorizontal;
    [FormerlySerializedAs("connection_line_diagonal")] public Image connectionLineDiagonal;
    

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

