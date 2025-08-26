using System;
using UnityEngine;
using TMPro;

public class MinigameScript : MonoBehaviour
{
    public GameObject minigamePanel;
    public GameObject player;
    public GameObject treeButton;
    public GameObject keys;

    public void HideMinigamePanel()
    {
        Debug.Log("hideMinigame button pressed");
        minigamePanel.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = true;
        InputBlocker.Instance.UnblockInput(); 
        // treeButton.SetActive(true);
        keys.SetActive(true);
    }
    
    public void ShowMinigamePanel()
    {
        Debug.Log("showMinigame button pressed");
        InputBlocker.Instance.BlockInput(); 
        minigamePanel.SetActive(true);
        player.GetComponent<PlayerMovement>().enabled = false;
        // treeButton.SetActive(false);
        keys.SetActive(false);
        Debug.Log("keys: " + keys.activeSelf);
    }
}
