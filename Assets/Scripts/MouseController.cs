using UnityEngine;

// everything the mouse can do
public class ActionController : MonoBehaviour
{
    public Vector3 MousePosition;
    void onMouseDown()
    {
        print("Mouse button clicked");
        print(Input.mousePosition);
    }
}
