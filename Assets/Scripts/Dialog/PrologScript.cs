using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PrologScript : MonoBehaviour
{
    public GameObject prologPanel;
    [SerializeField] private TMP_Text textLabel;
    public GameObject player;
    private TypewriterEffect _typewriterEffect;
    
    public void Start()
    {
        _typewriterEffect = GetComponent<TypewriterEffect>();
        Debug.Log("showMonolog button pressed");
        prologPanel.SetActive(false);
        textLabel.text = "";
    }
    
    public IEnumerator ShowProlog(DialogLine dialogLine)
    {
        Debug.Log("showProlog method was called");
        prologPanel.SetActive(true);
        player.GetComponent<PlayerMovement>().enabled = false;
        InputBlocker.Instance.BlockInput(); 
        yield return StartCoroutine(StepThroughDialogue(dialogLine));
    }
    
    //jede zeile des dialogs anzeigen lassen und am ende zwei buttons anzeigen lassen
    private IEnumerator StepThroughDialogue(DialogLine dialogLine)
    {
        // yield return new WaitForSeconds(1);
        foreach (string rawLine in dialogLine.dialogText)
        {
            string displayText = rawLine;
            textLabel.alignment = TextAlignmentOptions.Center;

            yield return _typewriterEffect.Run(displayText, textLabel);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space));
        }
        ClosePrologPanel();
    }

    // dialogfenster ausblenden, textfeld clearen und charakterbewegung aktivieren
    public void ClosePrologPanel()
    {
        prologPanel.SetActive(false);
        textLabel.text = string.Empty;
        player.GetComponent<PlayerMovement>().enabled = true;
        InputBlocker.Instance.UnblockInput();
    }
}
