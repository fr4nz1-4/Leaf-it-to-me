using System.Collections;
using UnityEngine;
using TMPro;

public class SpeechbubbleScript : MonoBehaviour
{
    public GameObject Speechbubble;
    [SerializeField] private TMP_Text textLabel;
    private TypewriterEffect typewriterEffect;
    [SerializeField] private GameObject player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        // Debug.Log("showSpeechbubble button pressed");
        Speechbubble.SetActive(false);
        textLabel.text = "";
    }

    public void ShowSpeechbubble(string text)
    {
        Speechbubble.SetActive(true);
        player.GetComponent<PlayerMovement>().enabled = false;
        StartCoroutine(StepThroughDialogue(text));
    }
    
    private IEnumerator StepThroughDialogue(string text)
    {
        yield return typewriterEffect.Run(text, textLabel);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        textLabel.text = string.Empty;
        
        // evtl noch buttons nur einblenden, wenn sie zugewiesen sind 
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        
        HideSpeechbubble();
    }
    
    // dialogfenster ausblenden, textfeld clearen und charakterbewegung aktivieren
    public void HideSpeechbubble()
    {
        Speechbubble.SetActive(false);
        textLabel.text = string.Empty;
        player.GetComponent<PlayerMovement>().enabled = true;
    }
}
