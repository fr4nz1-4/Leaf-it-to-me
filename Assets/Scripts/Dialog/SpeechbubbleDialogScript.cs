using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class SpeechbubbleDialogScript : MonoBehaviour
{
    [SerializeField] private GameObject _left_speechbubble;
    [SerializeField] private GameObject _right_speechbubble;
    private TypewriterEffect _typewriterEffect;
    [SerializeField] private TMP_Text left_textLabel;
    [SerializeField] private TMP_Text right_textLabel;
    public GameObject player;
    private bool _skipRequested = false;
    private bool _buttonClicked = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _typewriterEffect = GetComponent<TypewriterEffect>();
        HideSpeechbubbles();
        // ShowDialogue(dialogLine);
    }

    public void ShowDialogue(DialogLine dialogLine)
    {
        InputBlocker.Instance.BlockInput(); 
        left_textLabel.text = String.Empty;
        right_textLabel.text = String.Empty;
        _left_speechbubble.SetActive(true);
        _right_speechbubble.SetActive(true);
        player.GetComponent<PlayerMovement>().enabled = false;
        StartCoroutine(StepThroughDialogue(dialogLine));
    }
    
    private IEnumerator StepThroughDialogue(DialogLine dialogLine)
    {
        _skipRequested = false;
        var useLeftLabel = true;
        
        // yield return new WaitForSeconds(1);
        foreach (var rawLine in dialogLine.dialogText)
        {
            var displayText = rawLine;
            left_textLabel.alignment = TextAlignmentOptions.Center;
            
            if (_skipRequested) break;

            TMP_Text activeLabel = null;
            if (rawLine.StartsWith("@"))
            {
                left_textLabel.text = string.Empty;
                displayText = rawLine.Substring(1); // 1. Zeichen entfernen
                activeLabel = left_textLabel;
                useLeftLabel = true; 
            } 
            else if (rawLine.StartsWith("#"))
            {
                right_textLabel.text = string.Empty;
                displayText = rawLine.Substring(1); // 1. Zeichen entfernen
                activeLabel = right_textLabel;
                useLeftLabel = false; 
            } else
            {
                // Abwechselnd linkes/rechtes Label
                activeLabel = useLeftLabel ? left_textLabel : right_textLabel;
            }
            
            yield return _typewriterEffect.Run(displayText, activeLabel);
            // activeLabel.text = "hahahahahha";
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || _skipRequested);
            // useLeftLabel = !useLeftLabel;
        }
        // evtl noch buttons nur einblenden, wenn sie zugewiesen sind
        // yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        
        HideSpeechbubbles();
    }
    
    // dialogfenster ausblenden, textfeld clearen und charakterbewegung aktivieren
    public void HideSpeechbubbles()
    {
        _buttonClicked = true;
        _left_speechbubble.SetActive(false);
        _right_speechbubble.SetActive(false);
        left_textLabel.text = string.Empty;
        player.GetComponent<PlayerMovement>().enabled = true;
        InputBlocker.Instance.UnblockInput(); 
    }
}
