using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;
    public TMP_Text dialogText;
    public Button startMinigameButton;
    public Button quitDialogButton;

    private string[] dialogLines = { "Hello! Do you want to play Rock Paper Scissors with me?" };

    private int currentLine = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // dialogPanel.SetActive(true);
        dialogText.text = dialogLines[currentLine];
        startMinigameButton.onClick.AddListener(NextLine);
        quitDialogButton.onClick.AddListener(quitDialog);
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine < dialogLines.Length)
        {
            dialogText.text = dialogLines[currentLine];
        }
        else
        {
            dialogPanel.SetActive(false);
            // Hier Minigame starten
            FindFirstObjectByType<MinigameManager>().ShowMinigamePanel();
        }
    }
    
    public void StartDialog()
    {
        currentLine = 0;
        dialogPanel.SetActive(true);
        dialogText.text = dialogLines[currentLine];
    }

    public void quitDialog()
    {
        dialogPanel.SetActive(false);
    }
}
