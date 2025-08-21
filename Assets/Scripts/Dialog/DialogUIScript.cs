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
    private TypewriterEffect _typewriterEffect;
    
    [SerializeField] private TMP_Text textLabel;
    public GameObject speechbubble;
    [SerializeField] private TMP_Text speechbubbleTextLabel;
    public Button startMinigame;
    public Button leaveDialog;
    // public Button SkipDialog;
    public Image playerPortrait;
    public Image npcPortrait;
    
    public GameObject player;
    private bool _buttonClicked = false;
    private bool _skipRequested = false;
    private Vector3 _playerStandardScale;
    private Vector3 _npcStandardScale;

    // private GameObject canvas;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _typewriterEffect = GetComponent<TypewriterEffect>();
        CloseDialogPanel();
        // ShowDialogue(dialogLine);
        startMinigame.gameObject.SetActive(false);
        leaveDialog.gameObject.SetActive(false);
        // canvas = GameObject.Find("Canvas"); 
        _playerStandardScale = playerPortrait.transform.localScale;
        _npcStandardScale = npcPortrait.transform.localScale;
        speechbubble.gameObject.SetActive(false);
    }

    public void ShowDialogue(DialogLine dialogLine)
    {
        InputBlocker.Instance.BlockInput(); 
        playerPortrait.sprite = dialogLine.playerPortrait;
        npcPortrait.sprite = dialogLine.npcPortrait;
        dialogPanel.SetActive(true);
        startMinigame.gameObject.SetActive(false);
        leaveDialog.gameObject.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = false;
        StartCoroutine(StepThroughDialogue(dialogLine));
    }

    public void ShowDialogueWithoutButtons(DialogLine dialogLine)
    {
        InputBlocker.Instance.BlockInput(); 
        playerPortrait.sprite = dialogLine.playerPortrait;
        npcPortrait.sprite = dialogLine.npcPortrait;
        dialogPanel.SetActive(true);
        startMinigame.gameObject.SetActive(false);
        leaveDialog.gameObject.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = false;
        StartCoroutine(StepThroughDialogueWithoutButtons(dialogLine));
    }
    
    //jede zeile des dialogs anzeigen lassen und am ende zwei buttons anzeigen lassen
    private IEnumerator StepThroughDialogue(DialogLine dialogLine)
    {
        _skipRequested = false;
        TMP_Text activeLabel = null;
        
        // yield return new WaitForSeconds(1);
        foreach (string rawLine in dialogLine.dialogText)
        {
            string displayText = rawLine;
            activeLabel = textLabel;
            textLabel.alignment = TextAlignmentOptions.Center;
            speechbubble.gameObject.SetActive(false);

            if (_skipRequested) break;

            if (rawLine.StartsWith("@"))
            {
                displayText = rawLine.Substring(1); // 1. Zeichen entfernen
                // textLabel.alignment = TextAlignmentOptions.Left;
                playerPortrait.transform.localScale = _playerStandardScale * 1.04f;
            } 
            else if (rawLine.StartsWith("#"))
            {
                displayText = rawLine.Substring(1); // 1. Zeichen entfernen
                // textLabel.alignment = TextAlignmentOptions.Right;
                npcPortrait.transform.localScale = _npcStandardScale * 1.04f;
            }
            else if (rawLine.StartsWith("!"))
            {
                speechbubble.gameObject.SetActive(true);
                activeLabel = speechbubbleTextLabel;
                displayText = rawLine.Substring(1); // 1. Zeichen entfernen
            }
            
            yield return _typewriterEffect.Run(displayText, activeLabel);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || _skipRequested);
            playerPortrait.transform.localScale = _playerStandardScale;
            npcPortrait.transform.localScale = _npcStandardScale;
            textLabel.text = string.Empty;
        }
        
        // evtl noch buttons nur einblenden, wenn sie zugewiesen sind 
        // yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        _buttonClicked = false;
        if (minigame != null)
        {
            startMinigame.gameObject.SetActive(true);
        }
        leaveDialog.gameObject.SetActive(true);
        yield return new WaitUntil(() => _buttonClicked);
        
        HideDialogPanel();
    }
    
    private IEnumerator StepThroughDialogueWithoutButtons(DialogLine dialogLine)
    {
        _skipRequested = false;

        // yield return new WaitForSeconds(1);
        foreach (string rawLine in dialogLine.dialogText)
        {
            string displayText = rawLine;
            textLabel.alignment = TextAlignmentOptions.Center;
            
            if (_skipRequested) break;

            if (rawLine.StartsWith("@"))
            {
                displayText = rawLine.Substring(1); // 1. Zeichen entfernen
                // textLabel.alignment = TextAlignmentOptions.Left;
                playerPortrait.transform.localScale = _playerStandardScale * 1.02f;
            } 
            else if (rawLine.StartsWith("#"))
            {
                displayText = rawLine.Substring(1); // 1. Zeichen entfernen
                // textLabel.alignment = TextAlignmentOptions.Right;
                npcPortrait.transform.localScale = _npcStandardScale * 1.03f;
            }
            
            yield return _typewriterEffect.Run(displayText, textLabel);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || _skipRequested);
            playerPortrait.transform.localScale = _playerStandardScale;
            npcPortrait.transform.localScale = _npcStandardScale;
            textLabel.text = string.Empty;
        }
        // evtl noch buttons nur einblenden, wenn sie zugewiesen sind
        // yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        
        CloseDialogPanel();
    }

    // dialogfenster ausblenden, Charakter kann sich aber nicht bewegen
    public void HideDialogPanel()
    {
        _buttonClicked = true;
        dialogPanel.SetActive(false);
        textLabel.text = string.Empty;
    }
    
    // dialogfenster ausblenden, textfeld clearen und charakterbewegung aktivieren
    public void CloseDialogPanel()
    {
        _buttonClicked = true;
        dialogPanel.SetActive(false);
        textLabel.text = string.Empty;
        player.GetComponent<PlayerMovement>().enabled = true;
        InputBlocker.Instance.UnblockInput(); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _skipRequested = true;
        }
    }

    public void startMinigameOnClick()
    {
        _buttonClicked = true;
        // minigame starten bzw methode aufrufen
        InputBlocker.Instance.BlockInput();
        minigame.ShowMinigamePanel();
        
        // gameObject.GetComponent<MinigameScript>().ShowMinigamePanel();
    }
    
}
