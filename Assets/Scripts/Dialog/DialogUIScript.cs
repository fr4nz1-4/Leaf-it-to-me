using System;
using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DialogUIScript : MonoBehaviour
{
    public GameObject dialogPanel;
    public MinigameScript minigame;
    // public DialogLine dialogLine;
    private TypewriterEffect typewriterEffect;
    
    [SerializeField] private TMP_Text textLabel;
    public Button startMinigame;
    public Button leaveDialog;
    // public Button SkipDialog;
    public Image playerPortrait;
    public Image npcPortrait;
    
    public GameObject player;
    private bool buttonClicked = false;
    private bool skipRequested = false;

    // private GameObject canvas;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        CloseDialogPanel();
        // ShowDialogue(dialogLine);
        startMinigame.gameObject.SetActive(false);
        leaveDialog.gameObject.SetActive(false);
        // canvas = GameObject.Find("Canvas"); 
    }

    public void ShowDialogue(DialogLine dialogLine)
    {
        playerPortrait.sprite = dialogLine.playerPortrait;
        npcPortrait.sprite = dialogLine.npcPortrait;
        dialogPanel.SetActive(true);
        startMinigame.gameObject.SetActive(false);
        leaveDialog.gameObject.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = false;
        StartCoroutine(StepThroughDialogue(dialogLine));
    }

    //jede zeile des dialogs anzeigen lassen und am ende zwei buttons anzeigen lassen
    private IEnumerator StepThroughDialogue(DialogLine dialogLine)
    {
        skipRequested = false;

        // yield return new WaitForSeconds(1);
        foreach (string rawLine in dialogLine.dialogText)
        {
            string displayText = rawLine;
            textLabel.alignment = TextAlignmentOptions.Center;
            
            if (skipRequested) break;

            if (rawLine.StartsWith("@"))
            {
                displayText = rawLine.Substring(1); // 1. Zeichen entfernen
                textLabel.alignment = TextAlignmentOptions.Left;
            } 
            else if (rawLine.StartsWith("#"))
            {
                displayText = rawLine.Substring(1); // 1. Zeichen entfernen
                textLabel.alignment = TextAlignmentOptions.Right;
            }
            
            yield return typewriterEffect.Run(displayText, textLabel);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || skipRequested);
            textLabel.text = string.Empty;
        }
        
        // evtl noch buttons nur einblenden, wenn sie zugewiesen sind 
        // yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        buttonClicked = false;
        if (minigame != null)
        {
            startMinigame.gameObject.SetActive(true);
        }
        leaveDialog.gameObject.SetActive(true);
        yield return new WaitUntil(() => buttonClicked);
        
        HideDialogPanel();
    }

    // dialogfenster ausblenden, Charakter kann sich aber nicht bewegen
    public void HideDialogPanel()
    {
        buttonClicked = true;
        dialogPanel.SetActive(false);
        textLabel.text = string.Empty;
    }
    
    // dialogfenster ausblenden, textfeld clearen und charakterbewegung aktivieren
    public void CloseDialogPanel()
    {
        buttonClicked = true;
        dialogPanel.SetActive(false);
        textLabel.text = string.Empty;
        player.GetComponent<PlayerMovement>().enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skipRequested = true;
        }
    }

    public void startMinigameOnClick()
    {
        buttonClicked = true;
        // minigame starten bzw methode aufrufen
        minigame.ShowMinigamePanel();
        
        // gameObject.GetComponent<MinigameScript>().ShowMinigamePanel();
    }
    
}
