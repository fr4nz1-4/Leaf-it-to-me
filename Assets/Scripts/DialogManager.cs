using TMPro;
using UnityEngine;
using UnityEngine.UI;

// opens DialogPanel, starts Dialog, contains all Dialog-Logic
public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;
    public TMP_Text dialogText;
    public Button startMinigame;
    public Button leaveDialog;
    public Image playerPortrait;
    public Image npcPortrait;
    
    public DialogLine dialogLine;
    public MinigameScript minigame;
    
    public bool isMinigameActive;
    
    private string[] dialogLinesText;
    private int currentLine = 0;
    
    public void Start()
    {
        Debug.Log("showMinigame button pressed");
        dialogPanel.SetActive(false);
        dialogText.text = "";
        isMinigameActive = true;
        startMinigame.onClick.AddListener(StartMinigame);
        leaveDialog.onClick.AddListener(quitDialog);
    }
    public void quitDialog()
    {
        dialogPanel.SetActive(false);
    }

    // Methode um Dialog zu starten und Zeilen nacheinander anzuzeigen
    void startDialog(DialogLine dialogLine)
    {
        // currentDialogLines = dialogLinesText;
        dialogLinesText = dialogLine.dialogLines;
        currentLine = 0;

        // Portraits während des Dialogs setzen
        playerPortrait.sprite = dialogLine.playerPortrait;
        npcPortrait.sprite = dialogLine.npcPortrait;
        
        dialogPanel.SetActive(true);
        startMinigame.gameObject.SetActive(false);
        leaveDialog.gameObject.SetActive(false);
        
        dialogText.text = dialogLinesText[currentLine];
    }

    void NextLine()
    {
        currentLine++;
        // wenn Ende des Dialogs noch nicht erreicht, immer nächste Zeile anzeigen
        if (currentLine < dialogLinesText.Length)
        {
            dialogText.text = dialogLinesText[currentLine];
        }
        else
        {
            // Dialog beendet, Buttons einblenden
            startMinigame.gameObject.SetActive(true);
            leaveDialog.gameObject.SetActive(true);
        }
    }

    void StartMinigame()
    {
        minigame.ShowMinigamePanel();
    }
    
}
