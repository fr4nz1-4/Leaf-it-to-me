using UnityEngine;

// everything the mouse can do
public class MouseController : MonoBehaviour
{
    public Vector3 mousePos;
    public Vector3 mousePosWorld;
    public Camera mainCamera;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("Mouse button clicked");
            mousePos = Input.mousePosition;
            print("ScreenSpace: " + Input.mousePosition);
            mousePosWorld = mainCamera.ScreenToWorldPoint(mousePos);
            print("WorldSpace: " + mousePosWorld);
        }
    }
}