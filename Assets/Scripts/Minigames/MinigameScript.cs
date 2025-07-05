using UnityEngine;
using TMPro;

public class MinigameScript : MonoBehaviour
{
    public GameObject minigamePanel;
    public TextMeshProUGUI resultText;
    public GameObject player;
    public TextMeshProUGUI playerCounter;
    public TextMeshProUGUI enemyCounter;
    public static bool minigamePlayed = false;

    public void HideMinigamePanel()
    {
        Debug.Log("hideMinigame button pressed");
        minigamePanel.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = true;
        InputBlocker.Instance.UnblockInput(); 
    }
    
    public void ShowMinigamePanel()
    {
        Debug.Log("showMinigame button pressed");
        InputBlocker.Instance.BlockInput(); 
        minigamePanel.SetActive(true);
        resultText.text = "";
        playerCounter.text = "";
        enemyCounter.text = "";
        player.GetComponent<PlayerMovement>().enabled = false;
    }
}
