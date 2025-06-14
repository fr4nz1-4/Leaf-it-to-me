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
    private TypewriterEffect typewriterEffect;
    
    public void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        Debug.Log("showMonolog button pressed");
        monologPanel.SetActive(false);
        textLabel.text = "";
    }
    
    public void ShowMonolog(string text)
    {
        monologPanel.SetActive(true);
        player.GetComponent<PlayerMovement>().enabled = false;
        StartCoroutine(StepThroughDialogue(text));
    }
    
    //jede zeile des dialogs anzeigen lassen und am ende zwei buttons anzeigen lassen
    private IEnumerator StepThroughDialogue(string text)
    {
        yield return typewriterEffect.Run(text, textLabel);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        textLabel.text = string.Empty;
        
        // evtl noch buttons nur einblenden, wenn sie zugewiesen sind 
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        
        CloseMonologPanel();
    }
    
    // dialogfenster ausblenden, textfeld clearen und charakterbewegung aktivieren
    public void CloseMonologPanel()
    {
        monologPanel.SetActive(false);
        textLabel.text = string.Empty;
        player.GetComponent<PlayerMovement>().enabled = true;
    }
}
