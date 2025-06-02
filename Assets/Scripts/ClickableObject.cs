using System;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public MonologScript monologScript;
    public string text;

    private void OnMouseDown()
    {
        Debug.Log("You clicked on: " + gameObject.name);
        monologScript.ShowMonolog(text);
    }
}
