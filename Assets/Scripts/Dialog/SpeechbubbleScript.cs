using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class SpeechbubbleScript : MonoBehaviour
{
    [FormerlySerializedAs("Speechbubble")] public GameObject speechbubble;
    [SerializeField] private TMP_Text textLabel;
    private TypewriterEffect _typewriterEffect;
    [SerializeField] private GameObject player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _typewriterEffect = GetComponent<TypewriterEffect>();
        // Debug.Log("showSpeechbubble button pressed");
        speechbubble.SetActive(false);
        textLabel.text = "";
    }

    public void ShowSpeechbubble(string text)
    {
        speechbubble.SetActive(true);
        player.GetComponent<PlayerMovement>().enabled = false;
        StartCoroutine(StepThroughDialogue(text));
        InputBlocker.Instance.BlockInput(); 
    }
    
    private IEnumerator StepThroughDialogue(string text)
    {
        yield return _typewriterEffect.Run(text, textLabel);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        textLabel.text = string.Empty;
        
        // evtl noch buttons nur einblenden, wenn sie zugewiesen sind 
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        
        HideSpeechbubble();
    }
    
    // dialogfenster ausblenden, textfeld clearen und charakterbewegung aktivieren
    public void HideSpeechbubble()
    {
        speechbubble.SetActive(false);
        textLabel.text = string.Empty;
        player.GetComponent<PlayerMovement>().enabled = true;
        InputBlocker.Instance.UnblockInput(); 
    }
}
