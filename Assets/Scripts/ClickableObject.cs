using System;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("You clicked on: " + gameObject.name);
    }
}
