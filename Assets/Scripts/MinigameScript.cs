using UnityEngine;
using TMPro;

public class MinigameScript : MonoBehaviour
{
    public GameObject minigamePanel;
    public TextMeshProUGUI resultText;
    public GameObject player;
    
    public void closeMinigamePanel()
    {
        Debug.Log("hideMinigame button pressed");
        minigamePanel.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = true;
    }
    
    public void ShowMinigamePanel()
    {
        Debug.Log("showMinigame button pressed");
        minigamePanel.SetActive(true);
        resultText.text = "";
        player.GetComponent<PlayerMovement>().enabled = false;
    }
}
