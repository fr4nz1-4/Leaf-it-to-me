using UnityEngine;
using TMPro;

public class MinigameScript : MonoBehaviour
{
    public GameObject minigamePanel;
    public TextMeshProUGUI resultText;
    public bool isMinigameActive = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ShowMinigamePanel()
    {
        Debug.Log("showMinigame button pressed");
        minigamePanel.SetActive(true);
        resultText.text = "";
        isMinigameActive = true;
    }
}
