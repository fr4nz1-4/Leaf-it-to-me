using System;
using UnityEngine;

public class ClickableCharacter : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("You clicked on: " + gameObject.name);

        switch (gameObject.name)
        {
            case "flower_1_0":
                Debug.Log("I'm the flower fairy");
                break;
        }
    }
}