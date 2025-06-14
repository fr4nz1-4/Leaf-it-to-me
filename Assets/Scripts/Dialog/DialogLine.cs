using UnityEngine;

// creates a Dialog, you enter Pictures and Text (but in Unity)

[CreateAssetMenu(fileName = "DialogLine", menuName = "Dialog/DialogLine")]
public class DialogLine : ScriptableObject
{
    /*[TextArea(2, 5)] */ public string[] dialogText;

    public Sprite playerPortrait;
    public Sprite npcPortrait;
}