using System;
using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    private bool _isDefault = true;

    private void OnMouseDown()
    {
        if (_isDefault)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
            _isDefault = false;
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
            _isDefault = true;
        }
    }
}
