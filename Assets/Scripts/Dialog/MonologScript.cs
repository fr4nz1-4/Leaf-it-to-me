using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MonologScript : MonoBehaviour
{
    public GameObject monologPanel;
    [SerializeField] private TMP_Text textLabel;
    public GameObject player;
    private TypewriterEffect _typewriterEffect;

    private void Awake()
    {
        _typewriterEffect = GetComponent<TypewriterEffect>();
        if (_typewriterEffect == null)
        {
            Debug.LogError("TypewriterEffect-Komponente fehlt am MonologScript-GameObject!");
        }
        if (textLabel == null)
        {
            Debug.LogError("textLabel ist nicht zugewiesen!");
        }           
    }

    public void Start()
    {
        if (monologPanel == null)
        {
            Debug.LogError("MonologPanel ist nicht zugewiesen!");
            return;
        }
        monologPanel.SetActive(false);
        textLabel.text = "";
    }
    
    public void ShowMonolog(string text)
    {
        monologPanel.SetActive(true);
        Debug.Log("monologpanel: " + monologPanel.activeSelf);
        Debug.Log("Panel Position: " + monologPanel.GetComponent<RectTransform>().anchoredPosition);
        Debug.Log("Text Position: " + textLabel.GetComponent<RectTransform>().anchoredPosition);
        player.GetComponent<PlayerMovement>().enabled = false;
        InputBlocker.Instance.BlockInput();
        Debug.Log("inputblocker: " + InputBlocker.Instance.IsBlocked);
        StartCoroutine(StepThroughDialogue(text));
    }
    
    //jede zeile des dialogs anzeigen lassen und am ende zwei buttons anzeigen lassen
    private IEnumerator StepThroughDialogue(string text)
    {
        Debug.Log("in StepThroughDialogue method");
        yield return _typewriterEffect.Run(text, textLabel);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space));
        textLabel.text = string.Empty;
        
        // evtl noch buttons nur einblenden, wenn sie zugewiesen sind 
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space));
        
        CloseMonologPanel();
    }
    
    // dialogfenster ausblenden, textfeld clearen und charakterbewegung aktivieren
    public void CloseMonologPanel()
    {
        monologPanel.SetActive(false);
        textLabel.text = string.Empty;
        player.GetComponent<PlayerMovement>().enabled = true;
        InputBlocker.Instance.UnblockInput();
    }
}
