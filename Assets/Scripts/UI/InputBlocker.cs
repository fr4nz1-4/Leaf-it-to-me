using UnityEngine;

public class InputBlocker : MonoBehaviour
{
    public static InputBlocker Instance { get; private set; }

    private void Awake()
    {
        // Singleton-Instanz setzen
        if (Instance != null && Instance != this)
        {
            gameObject.SetActive(false);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject); // optional
    }

    public bool IsBlocked { get; private set; } = false;

    public void BlockInput()
    {
        IsBlocked = true;
        Debug.Log("Input wurde blockiert.");
    }

    public void UnblockInput()
    {
        IsBlocked = false;
        Debug.Log("Input wurde freigegeben.");
    }
}