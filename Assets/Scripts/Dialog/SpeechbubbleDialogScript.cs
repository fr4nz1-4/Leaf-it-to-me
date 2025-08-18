using UnityEngine;

public class SpeechbubbleDialogScript : MonoBehaviour
{
    public GameObject dialogPanel;
    private TypewriterEffect _typewriterEffect;
    [SerializeField] private TMP_Text textLabel;
    public GameObject player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _typewriterEffect = GetComponent<TypewriterEffect>();
        CloseDialogPanel();
        // ShowDialogue(dialogLine);
    }

    public void ShowDialogueWithoutButtons(DialogLine dialogLine)
    {
        InputBlocker.Instance.BlockInput(); 
        dialogPanel.SetActive(true);
        player.GetComponent<PlayerMovement>().enabled = false;
        StartCoroutine(StepThroughDialogueWithoutButtons(dialogLine));
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
    
    // dialogfenster ausblenden, textfeld clearen und charakterbewegung aktivieren
    public void CloseDialogPanel()
    {
        _buttonClicked = true;
        dialogPanel.SetActive(false);
        textLabel.text = string.Empty;
        player.GetComponent<PlayerMovement>().enabled = true;
        InputBlocker.Instance.UnblockInput(); 
    }
}
