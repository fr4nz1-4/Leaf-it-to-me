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

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
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
        Debug.Log("You hovered over: " + gameObject.name);
        ColorUtility.TryParseHtmlString("#BCBCBC", out Color color);
        spriteRenderer.color = color;
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = Color.white;
    }
}
