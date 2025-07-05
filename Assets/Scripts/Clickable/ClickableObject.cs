using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ColorUtility = UnityEngine.ColorUtility;

public class ClickableObject : MonoBehaviour
{
    public MonologScript monologScript;
    public string text;
    private AudioSource audioSource;
    public SpeechbubbleScript SpeechbubbleScript;
    private SpriteRenderer spriteRenderer;
    private Vector3 standardScale;
    private Vector3 transformedScale;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        standardScale = gameObject.transform.localScale;
    }

    private void OnMouseDown()
    {
        if (InputBlocker.Instance.IsBlocked) return;
        
        Debug.Log("You clicked on: " + gameObject.name);
        
        if (monologScript != null)
        {
            monologScript.ShowMonolog(text);
        }

        if (audioSource != null)
        {
            Debug.Log("play audiosource");
            audioSource.Play();
        }
        
        if (SpeechbubbleScript != null)
        {
            SpeechbubbleScript.ShowSpeechbubble(text);
        }
    }
    
    private void OnMouseOver()
    {
        if (InputBlocker.Instance.IsBlocked) return;

        Debug.Log("You hovered over: " + gameObject.name);
        ColorUtility.TryParseHtmlString("#BCBCBC", out Color color);
        spriteRenderer.color = color;

        // if (gameObject.name == "crayons" || gameObject.name == "paper on floor")
        // {
        //     return;
        // }
        // gameObject.transform.localScale = standardScale * 1.02f;
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = Color.white;
        // gameObject.transform.localScale = standardScale;
    }
}
